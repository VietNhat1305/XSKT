using MongoDB.Driver;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Extensions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Interfaces.NghiepVu;
using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Models.PagingParam;
using XoSoKienThiet.Services.Core;

namespace XoSoKienThiet.Services.NghiepVu
{

    public class LoaiAnhService : BaseService, ILoaiAnhService
    {
        private DataContext _context;
        private BaseMongoDb<LoaiAnh, string> BaseMongoDb;
        protected IMongoCollection<LoaiAnh> _collection;
        public LoaiAnhService(
            DataContext context,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<LoaiAnh, string>(_context.LOAIANH);
           
        }

        public async Task<dynamic> Create(LoaiAnh model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);             

                var content = new LoaiAnh()
                {
                    Name = model.Name,                   
                    ThuTu = model.ThuTu,
                    File = model.File
                };

                content.CreatedBy = CurrentUser.UserName;
                ResultBaseMongo<LoaiAnh> result;

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

        public async Task<dynamic> Update(LoaiAnh model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                var content = _context.LOAIANH.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();

                if (content == default)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

                content.Name = model.Name;
                content.ThuTu = model.ThuTu;
                content.File = model.File;
                

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

        public async Task<PagingModel<LoaiAnh>> GetPagingCore(PagingParam param)
        {
            PagingModel<LoaiAnh> result = new PagingModel<LoaiAnh>();
            var builder = Builders<LoaiAnh>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Eq("IsDeleted", false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Name.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            string sortBy = nameof(IIdEntity<string>.ModifiedAt);
            if (param.SortDesc != default)
            {
                sortBy = param.SortBy;
            }
            else
            {
                param.SortDesc = true;
            }
            result.TotalRows = await _context.LOAIANH.CountDocumentsAsync(filter);
            result.Data = await _context.LOAIANH.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<LoaiAnh>
                        .Sort.Descending(sortBy)
                    : Builders<LoaiAnh>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            foreach (var item in result.Data)
            {
                var count = await _context.THUVIENANH.Find(x => x.LoaiAnhId == item.Id).ToListAsync();
                item.Count = count.Count();
            }
            return result;
        }
    }
}