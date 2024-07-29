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
using XoSoKienThiet.Models.PagingParam;
using XoSoKienThiet.Services.Core;

namespace XoSoKienThiet.Services.NghiepVu
{

    public class LinkYtbService : BaseService, ILinkYtbService
    {
        private DataContext _context;
        private BaseMongoDb<LinkYtb, string> BaseMongoDb;
        private readonly IFileMinioService _fileMinioService;
        public LinkYtbService(
            DataContext context,
            IHttpContextAccessor contextAccessor,
            IFileMinioService fileMinioService) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<LinkYtb, string>(_context.LINKYTB);
            _fileMinioService = fileMinioService;
        }

        public async Task<dynamic> Create(LinkYtb model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
             

                var content = new LinkYtb()
                {
                    Name = model.Name,
                    ThuTu = model.ThuTu,
                    Link = model.Link,
                 
                };

                content.CreatedBy = CurrentUser.UserName;
                ResultBaseMongo<LinkYtb> result;

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

        public async Task<dynamic> Update(LinkYtb model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                var content = _context.LINKYTB.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();

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
        public async Task<dynamic> Tinkhac(PagingParam pagingParam)
        {
            try
            {
                var query = _context.LINKYTB.Find(x => !x.IsDeleted);

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
        //public async Task<dynamic> GetAll()
        //{
        //    var model = await _context.LINKYTB.Find(x => !x.IsDeleted).FirstOrDefaultAsync();

        //    return model;

        //}
    }
}