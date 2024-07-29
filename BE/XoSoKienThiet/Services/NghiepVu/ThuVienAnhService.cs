using FluentValidation.Results;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Extensions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.NghiepVu;
using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Models.PagingParam;
using XoSoKienThiet.Services.Core;

namespace XoSoKienThiet.Services.NghiepVu
{

    public class ThuVienAnhService : BaseService, IThuVienAnhService
    {
        private DataContext _context;
        private BaseMongoDb<ThuVienAnh, string> BaseMongoDb;
        public ThuVienAnhService(
            DataContext context,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<ThuVienAnh, string>(_context.THUVIENANH);            
        }

        public async Task<dynamic> Create(ThuVienAnh model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

                //ValidationResult validationResult = new ThuVienAnhValidation().Validate(model);
                //if (!validationResult.IsValid)
                //    throw new ResponseMessageException().WithValidationResult(validationResult);

                var content = new ThuVienAnh()
                {
                    Name = model.Name,
                    File = model.File,
                    ThuTu = model.ThuTu,
                    LoaiAnhId = model.LoaiAnhId
                };

                content.CreatedBy = CurrentUser.UserName;
                ResultBaseMongo<ThuVienAnh> result;

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

        public async Task<dynamic> Update(ThuVienAnh model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                var content = _context.THUVIENANH.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();

                if (content == default)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

                //ValidationResult validationResult = new ThuVienAnhValidation().Validate(model);
                //if (!validationResult.IsValid)
                //    throw new ResponseMessageException().WithValidationResult(validationResult);

                content.Name = model.Name;
                content.File = model.File;
                content.ThuTu = model.ThuTu;
                content.LoaiAnhId = model.LoaiAnhId;

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
        public async Task<dynamic> Tinkhac(PagingParam pagingParam)
        {
            try
            {
                var query = _context.THUVIENANH.Find(x => !x.IsDeleted);

                var totalRecords = await query.CountDocumentsAsync();

                var relatedData = await query
                    .SortByDescending(x => x.CreatedAt)
                    .Skip(pagingParam.Skip)
                    .Limit(pagingParam.Limit)
                    .ToListAsync();

                Console.WriteLine(relatedData);

                return new
                {
                    TotalRecords = totalRecords,
                    Data = relatedData
                };
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException().WithCode(e.ResultCode).WithMessage(e.ResultString).WithDetail(e.Error);
            }
            catch (Exception ex)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(ex.Message);
            }
        }

        public async Task<dynamic> GetByIdPhanLoai(string id)
        {
            try
            {
                var entity = await _context.THUVIENANH.Find(x => !x.IsDeleted && x.LoaiAnhId == id).ToListAsync();
                return entity;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException().WithCode(e.ResultCode).WithMessage(e.ResultString).WithDetail(e.Error);
            }
            catch (Exception ex)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(ex.Message);
            }
        }
    }
}