using AutoMapper;
using POS.Application.Commons.Base;
using POS.Application.Dtos.Category.Response;
using POS.Application.Dtos.DocumentType.Response;
using POS.Application.Interfaces;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class DocumentTypeApplication : IDocumentTypeApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DocumentTypeApplication(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IEnumerable<DocumentTypeResponseDto>>> ListDocumentTypes()
        {
            var response = new BaseResponse<IEnumerable<DocumentTypeResponseDto>>();

            try
            {
                var documentTypes = await _unitOfWork.DocumentType.ListDocumentTypes();

                if (documentTypes is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<IEnumerable<DocumentTypeResponseDto>>(documentTypes);
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
    }
}
