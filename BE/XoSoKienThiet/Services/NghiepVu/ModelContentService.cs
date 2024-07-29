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
using MongoDB.Bson.Serialization;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Models.PagingParam;
using MongoDB.Bson;
using XoSoKienThiet.FromBodyModels;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.Models.Appsettings;
using System.Globalization;
using XoSoKienThiet.Validation.NghiepVu;

namespace XoSoKienThiet.Services.NghiepVu
{

    public class ModelContentService : BaseService, IModelContentService
    {
        protected IMongoCollection<ModelContent> _collection;
        private DataContext _context;
        private MenuId _settings;
        private BaseMongoDb<ModelContent, string> BaseMongoDb;
        protected ProjectionDefinition<ModelContent, BsonDocument> projectionDefinition = Builders<ModelContent>.Projection
            .Exclude("ModifiedAt")
            .Exclude("CreatedBy")
            .Exclude("ModifiedBy")
            .Exclude("IsDeleted")
            .Exclude("CreatedAtString")
            .Exclude("PasswordSalt")
            .Exclude("PasswordHash")
            .Exclude("Sort")
            .Exclude("UnsignedName");

        public ModelContentService(
      DataContext context,
      IHttpContextAccessor contextAccessor) :
      base(context, contextAccessor)
        {
            _context = context;
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration _configuration = builder.Build();
            BaseMongoDb = new BaseMongoDb<ModelContent, string>(_context.MODELCONTENT);
            _settings = _configuration.GetSection("MenuId").Get<MenuId>();
        }

        public async Task<dynamic> Create(ModelContent model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                /*ValidationResult validationResult = new ModelContentValidation(_context).Validate(model);
                if (!validationResult.IsValid)
                    throw new ResponseMessageException().WithValidationResult(validationResult);*/


                var content = new ModelContent()
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
                ResultBaseMongo<ModelContent> result;

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

        public async Task<dynamic> Update(ModelContent model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                var content = _context.MODELCONTENT.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();

                if (content == default)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

                //var CheckName = _context.CAPGIAYPHEPXAYDUNG.Find(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id).FirstOrDefault();
                //if (CheckName != default)
                //    throw new ResponseMessageException().WithException(DefaultCode.DATA_EXISTED);

                /*ValidationResult validationResult = new ModelContentValidation(_context).Validate(model);
                if (!validationResult.IsValid)
                    throw new ResponseMessageException().WithValidationResult(validationResult);*/

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

        public async Task<dynamic> Tinkhac(ContentPagingParam pagingParam)
        {
            try
            {
             
                var builder = Builders<ModelContent>.Filter;
                var filter = builder.Empty;

                string sortBy = pagingParam.SortBy != null ? FormatterString.HandlerSortBy(pagingParam.SortBy) : "CreatedAt";

                var relatedData = await _context.MODELCONTENT
                    .Find(x => !x.IsDeleted && x.IsPublic && x.Menu.Any(menuItem => menuItem.Id == pagingParam.MenuId) && x.Id != pagingParam.ContentId)
                .Sort(pagingParam.SortDesc
                        ? Builders<ModelContent>.Sort.Descending(sortBy)
                        : Builders<ModelContent>.Sort.Ascending(sortBy))
                    .Skip(pagingParam.Skip)
                    .Limit(pagingParam.Limit)
                    .ToListAsync();

                var result = new PagingModel<dynamic>
                {
                    Data = relatedData.ToList()
                };

                return result;
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





        public async Task<dynamic> GetByMenuId(ContentPagingParam pagingParam)
        {
            try
            {
                if (pagingParam == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

                if (pagingParam.MenuId == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

                PagingModel<dynamic> result = new PagingModel<dynamic>();
                var builder = Builders<ModelContent>.Filter;
                var filter = builder.Empty;
                filter = builder.And(filter, builder.Where(x => !x.IsDeleted && x.IsPublic));

                // Retrieve menuHoiDapId from app settings
                if (_settings != null && _settings.menuHoiDap != null)
                {
                    string menuHoiDapId = _settings.menuHoiDap;

                    MenuCongDan menu = _context.MENUCONGDAN.Find(x => !x.IsDeleted && x.Id == pagingParam.MenuId).FirstOrDefault();
                    if (!string.IsNullOrEmpty(pagingParam.MenuId) && menu != null)
                    {
                        var listId = _context.MENUCONGDAN.Find(x => !x.IsDeleted && (x.ParentId == pagingParam.MenuId || x.Id == pagingParam.MenuId))
                            .ToList().Select(x => x.Id).ToList();

                        if (listId != null && listId.Count > 1)
                        {
                            filter = builder.And(filter,
                                builder.ElemMatch(x => x.Menu,
                                    Builders<MenuCongDanShort>.Filter.In(f => f.Id, listId)
                                ));
                        }
                        else if (pagingParam.MenuId.Equals(menuHoiDapId))
                        {
                            filter = builder.And(filter,
                                builder.Where(x => x.Menu.Any(menuItem => menuItem.Id == menuHoiDapId && !string.IsNullOrEmpty(x.NoiDung)))
                            );
                        }
                        else if (menu.SoLuongTin == 1)
                        {
                            return _context.MODELCONTENT.Find(x => !x.IsDeleted && x.IsPublic && x.Menu.Any(menuItem => menuItem.Id == pagingParam.MenuId)).SortByDescending(x => x.CreatedAt).FirstOrDefault();
                        }
                        else
                        {
                            filter = builder.And(filter, builder.Where(x => x.Menu.Any(menuItem => menuItem.Id == pagingParam.MenuId)));
                        }
                    }
                }
                result.TotalRows = await _context.MODELCONTENT.CountDocumentsAsync(filter);

                string sortBy = pagingParam.SortBy != null ? FormatterString.HandlerSortBy(pagingParam.SortBy) : "NgayXuatBan";

                var list = await _context.MODELCONTENT.Find(filter)
                    .Sort(pagingParam.SortDesc
                        ? Builders<ModelContent>.Sort.Descending(sortBy)
                        : Builders<ModelContent>.Sort.Ascending(sortBy))
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
                PagingModel<ModelContent> result = new PagingModel<ModelContent>();
                var builder = Builders<ModelContent>.Filter;
                var filter = builder.Empty;
                filter = builder.And(filter, builder.Where(x => !x.IsDeleted));
                if (!String.IsNullOrEmpty(pagingParam.Content))
                {
                    filter = builder.And(filter,
                        builder.Where(x =>
                            x.UnsignedName == FormatterString.ConvertToUnsign(pagingParam.Content) || x.NoiDung.Contains(pagingParam.Content)));
                }
                
                
                if (pagingParam.MenuId != null && pagingParam.MenuId != _settings.menuHoiDap)
                {
                    filter = builder.And(filter,
                        builder.ElemMatch(x => x.Menu, menu => menu.Id == pagingParam.MenuId)
                    );
                }
                else
                {
                    filter = builder.And(filter,
                        builder.ElemMatch(x => x.Menu, menu => menu.Id != _settings.menuHoiDap)
                    );
                }

                if (pagingParam.Year != default)
                {
                    filter = builder.And(filter,
                        builder.Where(x => x.NgayKy != null && x.NgayKy.Value.Year.ToString() == pagingParam.Year.ToString())
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



                result.TotalRows = await _context.MODELCONTENT.CountDocumentsAsync(filter);

                result.Data = await _context.MODELCONTENT.Find(filter)
                   .SortByDescending(x => x.NgayXuatBan)
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
        
        
           public async Task<dynamic> GetPagingHoiDap(ModelContentPagingParam pagingParam)
            {
                try
                {
                    PagingModel<ModelContent> result = new PagingModel<ModelContent>();
                    var builder = Builders<ModelContent>.Filter;
                    var filter = builder.Empty;
                    filter = builder.And(filter, builder.Where(x => !x.IsDeleted));
                    if (!String.IsNullOrEmpty(pagingParam.Content))
                    {
                        filter = builder.And(filter,
                            builder.Where(x =>
                                x.UnsignedName == FormatterString.ConvertToUnsign(pagingParam.Content) || x.NoiDung.Contains(pagingParam.Content)));
                    }
                
                
                        filter = builder.And(filter,
                            builder.ElemMatch(x => x.Menu, menu => menu.Id == _settings.menuHoiDap)
                        );



                        var a = _context.MODELCONTENT.CountDocumentsAsync(filter).Result;
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



                    result.TotalRows = await _context.MODELCONTENT.CountDocumentsAsync(filter);

                    result.Data = await _context.MODELCONTENT.Find(filter)
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

        public async Task<dynamic> GetPagingHoiDapCongDan(ModelContentPagingParam pagingParam)
        {
            try
            {
                PagingModel<ModelContent> result = new PagingModel<ModelContent>();
                var builder = Builders<ModelContent>.Filter;
                var filter = builder.Empty;
                filter = builder.And(filter, builder.Where(x => !x.IsDeleted && x.IsPublic && x.NoiDung != null && !x.NoiDung.Equals("")));
                if (!String.IsNullOrEmpty(pagingParam.Content))
                {
                    filter = builder.And(filter,
                        builder.Where(x =>
                            x.UnsignedName == FormatterString.ConvertToUnsign(pagingParam.Content) || x.NoiDung.Contains(pagingParam.Content)));
                }


                filter = builder.And(filter,
                    builder.ElemMatch(x => x.Menu, menu => menu.Id == _settings.menuHoiDap)
                );



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



                result.TotalRows = await _context.MODELCONTENT.CountDocumentsAsync(filter);

                result.Data = await _context.MODELCONTENT.Find(filter)
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

        public async Task<dynamic> CreateHoiDap(ModelContent model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                ValidationResult validationResult = new HoiDapValidation().Validate(model);
                if (!validationResult.IsValid)
                    throw new ResponseMessageException().WithValidationResult(validationResult);


                var content = new ModelContent()
                {
                    Name = model.Name,
                    HoVaTen = model.HoVaTen,
                    NoiDung = model.NoiDung,
                    SoDienThoai = model.SoDienThoai,
                    Email = model.Email,
                    DiaChi = model.DiaChi,
                    Menu = model.Menu,
                    MoTa = model.MoTa,
                    IsPublic = model.IsPublic,
                    Files = model.Files,
                };
                ResultBaseMongo<ModelContent> result;

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


        public async Task<dynamic> UpdateHoiDap(ModelContent model)
            {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                var content = _context.MODELCONTENT.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();

                if (content == default)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

                //var CheckName = _context.CAPGIAYPHEPXAYDUNG.Find(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id).FirstOrDefault();
                //if (CheckName != default)
                //    throw new ResponseMessageException().WithException(DefaultCode.DATA_EXISTED);

                ValidationResult validationResult = new HoiDapValidation().Validate(model);
                if (!validationResult.IsValid)
                    throw new ResponseMessageException().WithValidationResult(validationResult);

                content.Name = model.Name;
                content.NoiDung = model.NoiDung;
                content.HoVaTen = model.HoVaTen;
                content.SoDienThoai = model.SoDienThoai;
                content.Email = model.Email;
                content.DiaChi = model.DiaChi;
                content.Menu = model.Menu;
                content.MoTa = model.MoTa;
                content.IsPublic = model.IsPublic;
                content.Files = model.Files;

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



        public async Task<dynamic> GetTinTrangChu()
        {
             try
             {
                 var listMenu = _context.MENUCONGDAN.Find(x => !x.IsDeleted && x.IsTrangChu).ToList();

                 var result = new List<PostListModel>();
                 foreach (var item in listMenu)
                 {
                     PostListModel post = new PostListModel();
                     post.Id = item.Id;
                     post.Name = item.Name;
                     var builder = Builders<ModelContent>.Filter;
                     var filter = builder.Empty;
                     filter = builder.And(filter, builder.Where(x => !x.IsDeleted && x.IsPublic));
                     filter = builder.And(filter,
                         builder.ElemMatch(x => x.Menu, menu => menu.Id == item.Id)
                     );
                     post.DanhSach =   await _context.MODELCONTENT
                         .Find(filter)
                         .SortByDescending(x => x.CreatedAt)
                         .Limit(4)
                         .Project<ModelContentShort>(Projection.Projection_Post)
                         .ToListAsync();
                     
                     result.Add(post);
                 }

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
}