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

    public class LienKietService : BaseService, ILienKietService
    {
        private DataContext _context;
        private BaseMongoDb<LienKet, string> BaseMongoDb;
        private readonly IFileMinioService _fileMinioService;
        public LienKietService(
            DataContext context,
            IHttpContextAccessor contextAccessor,
            IFileMinioService fileMinioService) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<LienKet, string>(_context.LIENKET);
            _fileMinioService = fileMinioService;
        }

        public async Task<dynamic> Create(LienKet model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
             

                var content = new LienKet()
                {
                    Name = model.Name,
                    ThuTu = model.ThuTu,
                    Link = model.Link,
               
                };

                content.CreatedBy = CurrentUser.UserName;
                ResultBaseMongo<LienKet> result;

                result = await BaseMongoDb.CreateAsync(content);

                if (result.Entity.Id == default || !result.Success)
                    throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);

                return content;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.ResultString).WithDetail(e.Error);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("is not a valid 24 digit hex string."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                }

                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(ex.Message);
            }
        }

        public async Task<dynamic> Update(LienKet model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                var content = _context.LIENKET.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();

                if (content == default)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

                //var CheckName = _context.CAPGIAYPHEPXAYDUNG.Find(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id).FirstOrDefault();
                //if (CheckName != default)
                //    throw new ResponseMessageException().WithException(DefaultCode.DATA_EXISTED);

              

                content.Name = model.Name;
                content.ThuTu = model.ThuTu;
                content.Link = model.Link;
              

                var result = await BaseMongoDb.UpdateAsync(content);
                if (!result.Success)
                    throw new ResponseMessageException().WithException(DefaultCode.UPDATE_FAILURE);
                return content;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.ResultString).WithDetail(e.Error);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("is not a valid 24 digit hex string."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                }
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(ex.Message);
            }

        }

        //public async Task<dynamic> GetAll()
        //{
        //    var model = await _context.LIENKET.Find(x => !x.IsDeleted).FirstOrDefaultAsync();

        //    return model;

        //}
    }
}