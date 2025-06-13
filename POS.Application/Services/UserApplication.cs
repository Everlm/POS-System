using AutoMapper;
using POS.Application.Commons.Bases.Response;
using POS.Application.Dtos.User.Request;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.FileStorage;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Infrastructure.Persistences.Repositories;
using POS.Utilities.Static;
using WatchDog;
using BC = BCrypt.Net.BCrypt;

namespace POS.Application.Services
{
    internal class UserApplication : IUserApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAzureStorage _azureStorage;
        private readonly INotifierService _notifierService;


        public UserApplication(IUnitOfWork unitOfWork, IMapper mapper, IAzureStorage azureStorage, INotifierService notifierService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _azureStorage = azureStorage;
            _notifierService = notifierService;
        }

        public async Task<BaseResponse<bool>> UpdateUserAsync(UpdateUserRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var user = await _unitOfWork.User.UserByEmail(requestDto.Email);

                if (user is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                bool rolesChanged = false;

                var currentUserRoleIds = user.UserRoles
                    .Select(ur => ur.RoleId)
                    .Where(roleId => roleId.HasValue)
                    .Select(roleId => roleId.Value)
                    .ToList();

                var currentRoleIdsSet = new HashSet<int>(currentUserRoleIds);
                var newRoleIdsSet = new HashSet<int>(requestDto.Roles);

                IEnumerable<Role> newRolesEntities = new List<Role>();

                if (!currentRoleIdsSet.SetEquals(newRoleIdsSet))
                {
                    rolesChanged = true;
                    Console.WriteLine($"Roles del usuario {user.Email} han cambiado. Actualizando...");

                    _unitOfWork.UserRole.RemoveRange(user.UserRoles);

                    var rolesToAdd = new List<UserRole>();

                    newRolesEntities = await _unitOfWork.Role.GetRolesByIdsAsync(requestDto.Roles);

                    if (newRolesEntities.Count() != requestDto.Roles.Count)
                    {
                        transaction.Rollback();
                        response.IsSuccess = false;
                        response.Message = "Uno o más IDs de rol proporcionados no son válidos.";
                        return response;
                    }

                    foreach (var roleEntity in newRolesEntities)
                    {
                        rolesToAdd.Add(new UserRole
                        {
                            UserId = user.Id,
                            RoleId = roleEntity.RoleId
                        });
                    }

                    await _unitOfWork.UserRole.AddRangeAsync(rolesToAdd);
                }
                // --- Actualizar otras propiedades del usuario (si UpdateUserRequestDto las tiene) ---
                // user.UserName = requestDto.UserName ?? user.UserName; 
                // user.Email = requestDto.Email; 
                // _unitOfWork.User.Update(user);

                // --- Enviar la notificación de SignalR SI los roles cambiaron ---
                if (rolesChanged)
                {
                    List<string> newRoleNamesForNotification;

                    if (newRoleIdsSet.Any())
                    {
                        newRoleNamesForNotification = newRolesEntities.Select(r => r.Description!).ToList();
                    }
                    else
                    {
                        newRoleNamesForNotification = new List<string>();
                    }

                    await _notifierService.NotifyUserRolesChanged(user.Email!, newRoleNamesForNotification);
                    Console.WriteLine($"Notificación de roles enviada para {user.Email} con roles: {string.Join(", ", newRoleNamesForNotification)}");
                }

                await _unitOfWork.SaveChangesAsync();
                transaction.Commit();
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RegisterUser(UserRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var account = _mapper.Map<User>(requestDto);
                account.Password = BC.HashPassword(account.Password);

                if (requestDto.Image is not null)
                {
                    account.Image = await _azureStorage.SaveFile(AzureContainers.USERS, requestDto.Image);
                }

                response.Data = await _unitOfWork.User.RegisterAsync(account);

                if (response.Data)
                {
                    response.IsSuccess = true;
                    response.Message = ReplyMessage.MESSAGE_SAVE;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_FAILED;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

    }
}
