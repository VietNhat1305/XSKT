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
    public class DiemBanLoToService : BaseService, IDiemBanLoToService
    {
        private DataContext _context;
        private BaseMongoDb<DiemBanLoTo, string> BaseMongoDb;
        private readonly IFileMinioService _fileMinioService;
        public DiemBanLoToService(
            DataContext context,
            IHttpContextAccessor contextAccessor,
            IFileMinioService fileMinioService) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<DiemBanLoTo, string>(_context.DIEMBANLOTO);
            _fileMinioService = fileMinioService;
        }

        public async Task<dynamic> Create(DiemBanLoTo model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);


                var content = new DiemBanLoTo()
                {
                    Name = model.Name,
                    File = model.File,
                    ThuTu = model.ThuTu
                };

                content.CreatedBy = CurrentUser.UserName;
                ResultBaseMongo<DiemBanLoTo> result;

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

        public async Task<dynamic> Update(DiemBanLoTo model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                var content = _context.DIEMBANLOTO.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();

                if (content == default)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

                //var CheckName = _context.CAPGIAYPHEPXAYDUNG.Find(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id).FirstOrDefault();
                //if (CheckName != default)
                //    throw new ResponseMessageException().WithException(DefaultCode.DATA_EXISTED);



                content.Name = model.Name;
                content.File = model.File;
                content.ThuTu = model.ThuTu;

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
    }
}
