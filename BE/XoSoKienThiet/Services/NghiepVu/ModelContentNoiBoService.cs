using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Extensions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.NghiepVu;
using XoSoKienThiet.Models.Appsettings;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Models.PagingParam;
using XoSoKienThiet.Services.Core;

namespace XoSoKienThiet.Services.NghiepVu;

public class ModelContentNoiBoService : BaseService, IModelContentNoiBoService
{
    private DataContext _context;
    private MenuId _settings;
    private BaseMongoDb<modelContentNoiBo, string> BaseMongoDb;
    protected ProjectionDefinition<modelContentNoiBo, BsonDocument> projectionDefinition = Builders<modelContentNoiBo>.Projection
        .Exclude("ModifiedAt")
        .Exclude("CreatedBy")
        .Exclude("ModifiedBy")
        .Exclude("IsDeleted")
        .Exclude("CreatedAtString")
        .Exclude("PasswordSalt")
        .Exclude("PasswordHash")
        .Exclude("Sort")
        .Exclude("UnsignedName");
    public ModelContentNoiBoService(
        DataContext context,
        IHttpContextAccessor contextAccessor) :
        base(context, contextAccessor)
    {
        _context = context;
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfiguration _configuration = builder.Build();
        BaseMongoDb = new BaseMongoDb<modelContentNoiBo, string>(_context.MODELCONTENTNOIBO);
        _settings = _configuration.GetSection("MenuId").Get<MenuId>();
    }
    
      public async Task<dynamic> Create(modelContentNoiBo model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                // ValidationResult validationResult = new ModelContentNoiBoValidation(_context).Validate(model);
                // if (!validationResult.IsValid)
                //     throw new ResponseMessageException().WithValidationResult(validationResult);


                var content = new modelContentNoiBo()
                {
                    Name = model.Name,
                    NoiDung = model.NoiDung,
                    Files = model.Files,
                    NgayKy = model.NgayKy,
                    SoKiHieu = model.SoKiHieu,
                    Menu = model.Menu,
                    IsPublic = model.IsPublic,
                    FileImage = model.FileImage,
                    NgayXuatBan = model.NgayXuatBan,
                    TrichYeu = model.TrichYeu,
                    MoTa = model.MoTa,
                    ThuTu = model.ThuTu

                };

               // content.CreatedBy = CurrentUser.UserName;
                ResultBaseMongo<modelContentNoiBo> result;

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

        public async Task<dynamic> Update(modelContentNoiBo model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                var content = _context.MODELCONTENTNOIBO.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();

                if (content == default)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

                //var CheckName = _context.CAPGIAYPHEPXAYDUNG.Find(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id).FirstOrDefault();
                //if (CheckName != default)
                //    throw new ResponseMessageException().WithException(DefaultCode.DATA_EXISTED);

                // ValidationResult validationResult = new ModelContentNoiBoValidation(_context).Validate(model);
                // if (!validationResult.IsValid)
                //     throw new ResponseMessageException().WithValidationResult(validationResult);

                content.Name = model.Name;
                content.NoiDung = model.NoiDung;
                content.Files = model.Files;
                content.SoKiHieu = model.SoKiHieu;
                content.NgayKy = model.NgayKy;
                content.IsPublic = model.IsPublic;
                content.Menu = model.Menu;
                content.FileImage = model.FileImage;
                content.NgayXuatBan = model.NgayXuatBan;
                content.MoTa = model.MoTa;
                content.TrichYeu = model.TrichYeu;
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



    public async Task<dynamic> GetByMenuId(ContentPagingParam pagingParam)
    {
        try
        {
            if (pagingParam == default)
                throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

            if (pagingParam.MenuId == default)
                throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

            PagingModel<dynamic> result = new PagingModel<dynamic>();
            var builder = Builders<modelContentNoiBo>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => !x.IsDeleted && x.IsPublic));

            // Retrieve menuHoiDapId from app settings

                Menu menu = _context.MENU.Find(x => !x.IsDeleted && x.Id == pagingParam.MenuId).FirstOrDefault();

                if (!string.IsNullOrEmpty(pagingParam.MenuId) && menu != null)
                {
                    var listId = _context.MENU.Find(x => !x.IsDeleted && (x.ParentId == pagingParam.MenuId || x.Id == pagingParam.MenuId))
                        .ToList().Select(x => x.Id).ToList();

                    if (listId != null && listId.Count > 1)
                    {
                        filter = builder.And(filter,
                            builder.ElemMatch(x => x.Menu,
                                Builders<MenuCongDan1>.Filter.In(f => f.Id, listId)
                            ));
                    }               
                    else
                    {
                        filter = builder.And(filter, builder.Where(x => x.Menu.Any(menuItem => menuItem.Id == pagingParam.MenuId)));
                    }
                }
            

            result.TotalRows = await _context.MODELCONTENTNOIBO.CountDocumentsAsync(filter);

            string sortBy = pagingParam.SortBy != null ? FormatterString.HandlerSortBy(pagingParam.SortBy) : "CreatedAt";

            var list = await _context.MODELCONTENTNOIBO.Find(filter)
                .Sort(pagingParam.SortDesc
                    ? Builders<modelContentNoiBo>.Sort.Descending(sortBy)
                    : Builders<modelContentNoiBo>.Sort.Ascending(sortBy))
                .Skip(pagingParam.Skip)
                .Limit(pagingParam.Limit)
                .Project(projectionDefinition)
                .ToListAsync();

            result.Data = list.Select(x => BsonSerializer.Deserialize<ModelContent>(x)).ToList();

            return result;
        }
        catch (ResponseMessageException e)
        {
            new ResultMessageResponse().WithCode(e.ResultCode)
                .WithMessage(e.ResultString);
        }
        catch (Exception e)
        {
            if (e.Message.Contains("is not a valid 24 digit hex string."))
            {
                throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
            }

            if (e.Message.Contains("ObjectSerializer does not support BSON type 'Binary'."))
            {
                throw new ResponseMessageException().WithException(DefaultCode.SORT_BY_NOT_EXIST);
            }
            throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.Message);
        }
        return null;
    }



    public async Task<dynamic> GetPaging(ModelContentPagingParam pagingParam)
        {
            try
            {
                PagingModel<modelContentNoiBo> result = new PagingModel<modelContentNoiBo>();
                var builder = Builders<modelContentNoiBo>.Filter;
                var filter = builder.Empty;
                filter = builder.And(filter, builder.Where(x => !x.IsDeleted));
                if (!String.IsNullOrEmpty(pagingParam.Content))
                {
                    filter = builder.And(filter,
                        builder.Where(x =>
                            x.UnsignedName == FormatterString.ConvertToUnsign(pagingParam.Content) || x.NoiDung.Contains(pagingParam.Content)));
                }
                
                
                if (pagingParam.MenuId != null)
                {
                    filter = builder.And(filter,
                        builder.ElemMatch(x => x.Menu, menu => menu.Id == pagingParam.MenuId)
                    );
                }
                
                if (pagingParam.StartDate != null && pagingParam.EndDate != null)
                {
                    int startDate = FormatDate.ConvertDatetimeQG(pagingParam.StartDate?.AddDays(1));
                    int endDate = FormatDate.ConvertDatetimeQG(pagingParam.EndDate?.AddDays(1));
                    filter = filter & builder.Gte(x => x.CreatedAtString, startDate)
                                    & builder.Lte(x => x.CreatedAtString, endDate);
                }
                if (pagingParam.StartDate != null && pagingParam.EndDate == null)
                {
                    int startDate = FormatDate.ConvertDatetimeQG(pagingParam.StartDate?.AddDays(1));
                    filter = filter & builder.Gte(x => x.CreatedAtString, startDate);
                }

                if (pagingParam.StartDate == null && pagingParam.EndDate != null)
                {
                    int endDate = FormatDate.ConvertDatetimeQG(pagingParam.EndDate?.AddDays(1));
                    filter = filter & builder.Lte(x => x.CreatedAtString, endDate);
                }



                result.TotalRows = await _context.MODELCONTENTNOIBO.CountDocumentsAsync(filter);

                result.Data = await _context.MODELCONTENTNOIBO.Find(filter)
                   .SortByDescending(x => x.CreatedAt)
                   .Skip(pagingParam.Skip)
                   .Limit(pagingParam.Limit)
                   .ToListAsync();


                return result;
            }
            catch (ResponseMessageException e)
            {
                new ResultMessageResponse().WithCode(e.ResultCode)
                    .WithMessage(e.ResultString);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("is not a valid 24 digit hex string."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                }
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.Message);
            }
            return null;
        }
}