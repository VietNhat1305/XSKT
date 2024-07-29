using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Driver;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Extensions;
using XoSoKienThiet.FromBodyModels;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Interfaces.NghiepVu;
using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Services.Core;

namespace XoSoKienThiet.Services.NghiepVu
{

    public class HeaderService : BaseService, IHeaderService
    {
        private DataContext _context;
        private BaseMongoDb<Header, string> BaseMongoDb;
        private readonly IFileMinioService _fileMinioService;
        public HeaderService(
            DataContext context,
            IHttpContextAccessor contextAccessor,
            IFileMinioService fileMinioService) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<Header, string>(_context.HEADER);
            _fileMinioService = fileMinioService;
        }

        public async Task<dynamic> Create(Header file)
        {
            try
            {
                if (file == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                //ValidationResult validationResult = new HoiValidation().Validate(model);
                //if (!validationResult.IsValid)
                //    throw new ResponseMessageException().WithValidationResult(validationResult);
                var entity = _context.HEADER.Find(x => !x.IsDeleted ).FirstOrDefault();
                ResultBaseMongo<Header> result = null;
                if (entity == null )
                {
                    file.Id = BsonObjectId.GenerateNewId().ToString();
                    result = await BaseMongoDb.CreateAsync(file);
                }

                if (entity != null && file.Id == null)
                {
                    entity.IsDeleted = true;
                    result = await BaseMongoDb.UpdateAsync(entity);
                    file.Id = BsonObjectId.GenerateNewId().ToString();
                    result = await BaseMongoDb.CreateAsync(file);
                }
                else 
                {
                    result = await BaseMongoDb.UpdateAsync(file);
                }
                
                if (result == null || result.Entity.Id == default || !result.Success)
                {
                    throw new ResponseMessageException()
                        .WithException(DefaultCode.CREATE_FAILURE);
                }
                return null;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.ResultString).WithDetail(e.Error);
            }
        }

        
        public async Task<dynamic> GetAll()
        {
            var model = await _context.HEADER.Find(x => !x.IsDeleted).FirstOrDefaultAsync();

            return model;

        }
    }
}