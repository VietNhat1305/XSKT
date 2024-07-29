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

    public class ImageHomeService : BaseService, IImageHomeService
    {
        private DataContext _context;
        private BaseMongoDb<ImageHome, string> BaseMongoDb;
        private readonly IFileMinioService _fileMinioService;
        public ImageHomeService(
            DataContext context,
            IHttpContextAccessor contextAccessor,
            IFileMinioService fileMinioService) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<ImageHome, string>(_context.IMAGEHOME);
            _fileMinioService = fileMinioService;
        }

        public async Task<dynamic> Create(ImageHome model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
             

                var content = new ImageHome()
                {
                    Name = model.Name,
                    File = model.File,
                    MoTa = model.MoTa,
                    ThuTu = model.ThuTu
                };

                content.CreatedBy = CurrentUser.UserName;
                ResultBaseMongo<ImageHome> result;

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

        public async Task<dynamic> Update(ImageHome model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                var content = _context.IMAGEHOME.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();

                if (content == default)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

                //var CheckName = _context.CAPGIAYPHEPXAYDUNG.Find(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id).FirstOrDefault();
                //if (CheckName != default)
                //    throw new ResponseMessageException().WithException(DefaultCode.DATA_EXISTED);

              

                content.Name = model.Name;
                content.File = model.File;
                content.MoTa = model.MoTa;
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

        //public async Task<dynamic> GetAll()
        //{
        //    var model = await _context.IMAGEHOME.Find(x => !x.IsDeleted).FirstOrDefaultAsync();

        //    return model;

        //}
    }
}