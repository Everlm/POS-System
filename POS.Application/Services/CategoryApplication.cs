using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Ordering;
using POS.Application.Dtos.Category.Request;
using POS.Application.Dtos.Category.Response;
using POS.Application.Interfaces;
using POS.Application.Validators.Category;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class CategoryApplication : ICategoryApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CategoryValidator _validateRules;
        private readonly IOrderingQuery _orderingQuery;

        public CategoryApplication(IUnitOfWork unitOfWork, IMapper mapper, CategoryValidator validateRules, IOrderingQuery orderingQuery)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validateRules = validateRules;
            _orderingQuery = orderingQuery;
        }

        public async Task<BaseResponse<IEnumerable<CategoryResponseDto>>> ListCategories(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<CategoryResponseDto>>();

            try
            {
                var categoriesQueryable = _unitOfWork.Category.GetAllQueryable();

                var categories = ApplyFilters(categoriesQueryable, filters);
                filters.Sort ??= "Id";

                var items = await _orderingQuery
                    .Ordering(filters, categories, !(bool)filters.Download!)
                    .ToListAsync();
                
                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<CategoryResponseDto>>(items);
                response.TotalRecords = await categories.CountAsync();
                response.Message = ReplyMessage.MESSAGE_QUERY;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        private static IQueryable<Category> ApplyFilters(IQueryable<Category> query, BaseFiltersRequest filters)
        {
            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                query = filters.NumFilter switch
                {
                    1 => query.Where(x => x.Name!.Contains(filters.TextFilter)),
                    2 => query.Where(x => x.Description!.Contains(filters.TextFilter)),
                    _ => query
                };
            }

            if (filters.StateFilter is not null)
            {
                query = query.Where(x => x.State == filters.StateFilter);
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate) &&
                DateTime.TryParse(filters.StartDate, out var startDate) && DateTime.TryParse(filters.EndDate, out var endDate))
            {
                query = query.Where(x => x.AuditCreateDate >= startDate && x.AuditCreateDate <= endDate.AddDays(1));
            }

            return query;
        }


        public async Task<BaseResponse<IEnumerable<CategorySelectResponseDto>>> ListSelectCategories()
        {
            var response = new BaseResponse<IEnumerable<CategorySelectResponseDto>>();

            try
            {
                var categories = await _unitOfWork.Category.GetAllAsync();

                if (categories is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<IEnumerable<CategorySelectResponseDto>>(categories);
                    response.Message = ReplyMessage.MESSAGE_QUERY;

                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
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
        public async Task<BaseResponse<CategoryResponseDto>> GetCategoryById(int categoryId)
        {
            var response = new BaseResponse<CategoryResponseDto>();

            try
            {
                var category = await _unitOfWork.Category.GetByIdAsync(categoryId);

                if (category is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<CategoryResponseDto>(category);
                    response.Message = ReplyMessage.MESSAGE_QUERY;

                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
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
        public async Task<BaseResponse<bool>> RegisterCategory(CategoryRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var validationResult = await _validateRules.ValidateAsync(requestDto);

                if (!validationResult.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_VALIDATE;
                    response.Errors = validationResult.Errors;
                    return response;
                }

                var category = _mapper.Map<Category>(requestDto);
                response.Data = await _unitOfWork.Category.RegisterAsync(category);

                if (response.Data)
                {
                    response.IsSuccess = true;
                    response.Message += ReplyMessage.MESSAGE_SAVE;
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
        public async Task<BaseResponse<bool>> EditCategory(CategoryRequestDto requestDto, int categoryId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var categoryEdit = await GetCategoryById(categoryId);

                if (categoryEdit.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var category = _mapper.Map<Category>(requestDto);
                category.Id = categoryId;
                response.Data = await _unitOfWork.Category.EditAsync(category);

                if (response.Data)
                {
                    response.IsSuccess = true;
                    response.Message = ReplyMessage.MESSAGE_UPDATE;
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
        public async Task<BaseResponse<bool>> DeleteCategory(int categoryId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var category = await GetCategoryById(categoryId);

                if (category.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }

                response.Data = await _unitOfWork.Category.DeleteAsync(categoryId);

                if (response.Data)
                {
                    response.IsSuccess = true;
                    response.Message = ReplyMessage.MESSAGE_DELETE;
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
