using AutoMapper;
using POS.Application.Commons.Bases.Response;
using POS.Application.Dtos.User.Request;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.FileStorage;
using POS.Infrastructure.Persistences.Interfaces;
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


        public UserApplication(IUnitOfWork unitOfWork, IMapper mapper, IAzureStorage azureStorage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _azureStorage = azureStorage;
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
