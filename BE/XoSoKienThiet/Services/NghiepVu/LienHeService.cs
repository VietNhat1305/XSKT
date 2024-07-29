using MongoDB.Driver;
using FluentValidation;
using FluentValidation.Results;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Extensions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Services.Core;
using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Interfaces.NghiepVu;
using System.Xml.Linq;
using MongoDB.Bson;
using XoSoKienThiet.ViewModels;
using XoSoKienThiet.Models.Core;

namespace QuanLyKhuCongNghiep.Services.NghiepVu
{

    public class LienHeService : BaseService, ILienHeService
    {
        private DataContext _context;
        private BaseMongoDb<LienHeModel, string> BaseMongoDb;
        public LienHeService(
            DataContext context,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<LienHeModel, string>(_context.LIENHE);

        }

        public async Task<dynamic> Create(LienHeModel model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

                
                var entity = _context.LIENHE.Find(x => !x.IsDeleted ).FirstOrDefault();

                ValidationResult validationResult = new LienHeValidation().Validate(model);
                if (!validationResult.IsValid)
                    throw new ResponseMessageException().WithValidationResult(validationResult);

                var lienhe = new LienHeModel()
                {
                    Name = model.Name,
                    DiaChi = model.DiaChi,
                    SoDienThoai = model.SoDienThoai,
                    Fax = model.Fax,
                    Email = model.Email,
                    VanPhongDaiDien = model.VanPhongDaiDien,
                    MaSoThue = model.MaSoThue,
                    NguoiBienTap = model.NguoiBienTap,
                    ThuTu = model.ThuTu
                };
                lienhe.CreatedBy = CurrentUser.UserName;
                ResultBaseMongo<LienHeModel> result;
                if (entity == null)
                {
                    lienhe.Id = BsonObjectId.GenerateNewId().ToString();
                    result = await BaseMongoDb.CreateAsync(lienhe);
                }
                else
                {
                    lienhe.Id = model.Id;
                    result = await BaseMongoDb.UpdateAsync(lienhe);
                }
                if (result.Entity.Id == default || !result.Success)
                {
                    throw new ResponseMessageException()
                        .WithException(DefaultCode.CREATE_FAILURE);
                }

                return lienhe;
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
        
        

        public async Task<dynamic> GetAll()
        {

            var listLH = await _context.LIENHE.Find(x => !x.IsDeleted).FirstAsync();

            return listLH;
        }

    }
}