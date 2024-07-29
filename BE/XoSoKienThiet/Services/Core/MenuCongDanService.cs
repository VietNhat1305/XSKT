using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Driver;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Extensions;
using XoSoKienThiet.FromBodyModels;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.ViewModels;
using XoSoKienThiet.Services.Core;
using Microsoft.AspNetCore.Http;
using XoSoKienThiet.Models.Appsettings;
using FluentValidation;

namespace XoSoKienThiet.Services.Core
{
    public class MenuCongDanService : BaseService, IMenuCongDanService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private MenuId _settings;
        private DataContext _context;
        private BaseMongoDb<MenuCongDan, string> BaseMongoDb;
        private List<string> ListActionDefault = new List<string>() {"manage", "create", "update", "delete"};
        public MenuCongDanService(
            DataContext context,
            IHttpContextAccessor httpContextAccessor) :
            base(context, httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            IConfigurationBuilder builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration _configuration = builder.Build();
            BaseMongoDb = new BaseMongoDb<MenuCongDan, string>(_context.MENUCONGDAN);
            _settings = _configuration.GetSection("MenuId").Get<MenuId>();
        }

        public async Task<dynamic> Create(MenuCongDan model)
        {
            try
            {
                
            
            if (model == default)
                throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

                /*var menu = _context.MENU.Find(x => !x.IsDeleted &&  (x.Name == model.Name || x.Resource == FormatterString.
                    ConvertToUnsign(model.Name).
                    Replace(" ",""))).FirstOrDefault();*/
                /*if (menu != default)
                {
                    throw new ResponseMessageException()
                        .WithException(DefaultCode.DATA_EXISTED);
                }*/
           
                ValidationResult validationResult = new MenuCongDanValidation().Validate(model);
            if (!validationResult.IsValid)         
                throw new ResponseMessageException().WithValidationResult(validationResult);     
            var entity = new MenuCongDan
            {
                Id = BsonObjectId.GenerateNewId().ToString(),
                Name = model.Name,
                Path = model.Path,
                Icon = model.Icon,
                ParentId = model.ParentId,
                Sort = model.Sort,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                IsAnhDaiDien = model.IsAnhDaiDien,
                IsTepTin = model.IsTepTin,
                IsKyHieu = model.IsKyHieu,
                IsMoTa = model.IsMoTa,
                IsNgayKy = model.IsNgayKy,
                IsNgayXuatBan  = model.IsNgayXuatBan,
                IsNoiDung = model.IsNoiDung,
                IsTieuDe=model.IsTieuDe,
                IsTrichYeu  = model.IsTrichYeu,
                IsDanhMuc = model.IsDanhMuc,
                IsMenu = model.IsMenu,
                IsTrangChu = model.IsTrangChu,
                SoLuongTin = model.SoLuongTin,
                //show menu
                IsShowAnhDaiDien = model.IsShowAnhDaiDien,
                IsShowTepTin = model.IsShowTepTin,
                IsShowKyHieu = model.IsShowKyHieu,
                IsShowMoTa = model.IsShowMoTa,
                IsShowNgayKy = model.IsShowNgayKy,
                IsShowNgayXuatBan = model.IsShowNgayXuatBan,
                IsShowNoiDung = model.IsShowNoiDung,
                IsShowTieuDe = model.IsShowTieuDe,
                IsShowTrichYeu = model.IsShowTrichYeu,
         
            };
            if (model.ParentId != null)
            {
                var level = _context.MENUCONGDAN.Find(x => x.Id == model.ParentId).FirstOrDefault();
                if (level == null)
                    throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage("Đơn vị cha không đúng !");
                entity.Level= level.Level + 1; 
            }
           // entity.SetListAction(entity.Name);
            var result = await BaseMongoDb.CreateAsync(entity);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException()
                    .WithException(DefaultCode.CREATE_FAILURE);
            }
            return entity;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException()
                    .WithCode(DefaultCode.EXCEPTION)
                    .WithMessage(e.ResultString)
                    .WithDetail(e.Error);
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


        #region Cap Nhat Menu 

           public async Task<dynamic> Update(MenuCongDan model)
           {
                try{
                    if (model == default)
                        throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                var entity = _context.MENUCONGDAN.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();
                if (entity == default)
                    throw new ResponseMessageException()
                        .WithException(DefaultCode.DATA_NOT_FOUND);


       
                    ValidationResult validationResult = new MenuCongDanValidation().Validate(model);
                    if (!validationResult.IsValid)
                        throw new ResponseMessageException().WithValidationResult(validationResult);
                entity.Name = model.Name;
                entity.Path = model.Path;
                entity.Icon = model.Icon;
             //   entity.ListAction = model.ListAction;
                entity.ParentId = model.ParentId;
                entity.Level = model.Level;
                entity.Sort = model.Sort;
                entity.IsAnhDaiDien = model.IsAnhDaiDien;
                entity.IsTepTin = model.IsTepTin;
                entity.IsKyHieu = model.IsKyHieu;
                entity.IsMoTa = model.IsMoTa;
                entity.IsNgayKy = model.IsNgayKy;
                entity.IsNgayXuatBan = model.IsNgayXuatBan;
                entity.IsNoiDung = model.IsNoiDung;
                entity.IsTieuDe = model.IsTieuDe;
                entity.IsTrichYeu = model.IsTrichYeu;
                //show menu
                entity.IsShowAnhDaiDien = model.IsShowAnhDaiDien;
                entity.IsShowTepTin = model.IsShowTepTin;
                entity.IsShowKyHieu = model.IsShowKyHieu;
                entity.IsShowMoTa = model.IsShowMoTa;
                entity.IsShowNgayKy = model.IsShowNgayKy;
                entity.IsShowNgayXuatBan = model.IsShowNgayXuatBan;
                entity.IsShowNoiDung = model.IsShowNoiDung;
                entity.IsShowTieuDe = model.IsShowTieuDe;
                entity.IsShowTrichYeu = model.IsShowTrichYeu;

                entity.IsTrangChu = model.IsTrangChu;
                entity.IsDanhMuc = model.IsDanhMuc;
                entity.IsMenu = model.IsMenu;
                entity.SoLuongTin = model.SoLuongTin;
                entity.ModifiedAt = DateTime.Now;
              //  entity.SetListAction(entity.Name);
                if (model.IsShowTieuDe == false && model.IsTieuDe == true)
                {
                    model.IsTieuDe = false;
                    Console.WriteLine($"IsShowTieuDe: {model.IsShowTieuDe}, IsTieuDe: {model.IsTieuDe}");

                }
                else
                //if (model.IsTieuDe && model.IsShowTieuDe == true)
                {
                    model.IsShowTieuDe = true;
                }
             

                if (model.ParentId != null)
                {
                    var menu = _context.MENUCONGDAN.Find(x => x.Id == model.ParentId).FirstOrDefault();
                    if (menu == null)
                        throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage("Đơn vị cha không đúng !");
                    entity.Level= menu.Level + 1; 
                }
                var result = await BaseMongoDb.UpdateAsync(entity);
                if (!result.Success)
                    throw new ResponseMessageException().WithException(DefaultCode.UPDATE_FAILURE);
                return entity;
                }
                catch (ResponseMessageException e)
                {
                    throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.ResultString).WithDetail(e.Error);
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("is not a valid 24 digit hex string."))
                    {
                        throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                    }
                    throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.Message);
                }
        }

           
           public async Task<List<MenuTreeVMCongDan>> GetDanhMuc()
           {
               var list = await _context.MENUCONGDAN.Find(x => !x.IsDeleted && x.IsDanhMuc).ToListAsync();
               List<MenuTreeVMCongDan> listMenu = new List<MenuTreeVMCongDan>();
               foreach (var item in list)
               {
                   MenuTreeVMCongDan donVi = new MenuTreeVMCongDan(item);
                   listMenu.Add(donVi);
               }
               return listMenu;
           }

           #endregion
     
        
        
        
        
        
        public async Task<List<MenuTreeVMCongDan>> GetTreeList()
        {
            var listDonVi = await _context.MENUCONGDAN.Find(x => !x.IsDeleted && x.IsMenu )
                .SortBy(donVi => donVi.Sort).ToListAsync();
            var parents = listDonVi.Where(x => x.ParentId == null || x.ParentId.Equals("")).ToList();
            List<MenuTreeVMCongDan> list = new List<MenuTreeVMCongDan>();
            foreach (var item in parents)
            {
                MenuTreeVMCongDan donVi = new MenuTreeVMCongDan(item);

                list.Add(donVi);
                GetLoopItem(ref list, listDonVi, donVi);
            }
            return list;
        }


        public async Task<List<MenuTreeVMCongDan>> GetTreeListAD()
        {
            var listDonVi = await _context.MENUCONGDAN.Find(x => !x.IsDeleted)
                .SortBy(donVi => donVi.Sort).ToListAsync();

            var parents = listDonVi.Where(x => x.ParentId == null || x.ParentId.Equals("")).ToList();
            List<MenuTreeVMCongDan> list = new List<MenuTreeVMCongDan>();

            foreach (var item in parents)
            {
                MenuTreeVMCongDan donVi = new MenuTreeVMCongDan(item);

                donVi.IsTieuDe = item.IsTieuDe;
                donVi.IsMoTa = item.IsMoTa;
                donVi.IsTrichYeu = item.IsTrichYeu;
                donVi.IsKyHieu = item.IsKyHieu;
                donVi.IsNgayXuatBan = item.IsNgayXuatBan;
                donVi.IsAnhDaiDien = item.IsAnhDaiDien;
                donVi.IsNgayKy = item.IsNgayKy;
                donVi.IsNoiDung = item.IsNoiDung;
                donVi.IsTepTin = item.IsTepTin;
               
                list.Add(donVi);
                GetLoopItem(ref list, listDonVi, donVi);
            }

            return list;
        }
        
        
        
        
        
        public async Task<List<MenuTreeVMCongDan>> GetTreeListTBV()
        {
            var listDonVi = await _context.MENUCONGDAN.Find(x => !x.IsDeleted && x.Id != _settings.menuHoiDap)
                .SortBy(donVi => donVi.Sort).ToListAsync();

            var parents = listDonVi.Where(x => x.ParentId == null || x.ParentId.Equals("")).ToList();
            List<MenuTreeVMCongDan> list = new List<MenuTreeVMCongDan>();

            foreach (var item in parents)
            {
                MenuTreeVMCongDan donVi = new MenuTreeVMCongDan(item);

                donVi.IsTieuDe = item.IsTieuDe;
                donVi.IsMoTa = item.IsMoTa;
                donVi.IsTrichYeu = item.IsTrichYeu;
                donVi.IsKyHieu = item.IsKyHieu;
                donVi.IsNgayXuatBan = item.IsNgayXuatBan;
                donVi.IsAnhDaiDien = item.IsAnhDaiDien;
                donVi.IsNgayKy = item.IsNgayKy;
                donVi.IsNoiDung = item.IsNoiDung;
                donVi.IsTepTin = item.IsTepTin;

                list.Add(donVi);
                GetLoopItem(ref list, listDonVi, donVi);
            }

            return list;
        }

        private List<MenuTreeVM> GetLoopItem(ref List<MenuTreeVMCongDan> list, List<MenuCongDan> items, MenuTreeVMCongDan target)
        {
            
            var menu =  items.Where(x=>x.ParentId == target.Id).OrderBy(x=>x.Sort).ToList();
            if (menu.Count > 0)
            {
                target.Children = new List<MenuTreeVMCongDan>();
                foreach (var item in menu)
                {
                    MenuTreeVMCongDan itemDV = new MenuTreeVMCongDan(item);
                    target.Children.Add(itemDV);
                    // target.SubItems.Add(itemDV);
                    GetLoopItem(ref list, items, itemDV);
                }
            }
            return null;
        }

        
        
        
        public async Task<List<NavMenuVMCongDan>> GetTreeListMenuForCurrentUser(List<MenuCongDan> listMenu)
        {
            var parents = listMenu.Where(x => x.ParentId == null).OrderBy(x=>x.Sort).ToList();
            List<NavMenuVMCongDan> list = new List<NavMenuVMCongDan>();
            foreach (var item in parents) 
            {
                if (item != null && item.Id != null)
                {
                    NavMenuVMCongDan menu = new NavMenuVMCongDan(item);
                    list.Add(menu);
                    item.IsChecked = true;
                   
                    UpdateMenu(listMenu, item);
                    GetLoopItemSubItems(ref list, listMenu, menu);  
                }
             
            }
            var remaining = listMenu.Where(x => x.IsChecked == false).ToList();
            foreach (var item in remaining) 
            {
                if (item != null && item.Id != null)
                {
                    NavMenuVMCongDan menu = new NavMenuVMCongDan(item);
                    list.Add(menu);
                    item.IsChecked = true;
                  
                    UpdateMenu(listMenu, item);
                    GetLoopItemSubItems(ref list, listMenu, menu);
                        
                }
              
            }
            return list;
        }
        
        
        
        
        public async Task<List<NavMenuVMCongDan>> GetTreeListMenuAll()
        {

            var listMenu = await _context.MENUCONGDAN.Find(_ => _.IsDeleted != true).SortBy(donVi => donVi.Level).ToListAsync();
            var parents = listMenu.Where(x => x.ParentId == null || x.ParentId.Equals("")).ToList();
            
            List<NavMenuVMCongDan> list = new List<NavMenuVMCongDan>();
            foreach (var item in parents)
            {
                NavMenuVMCongDan menu = new NavMenuVMCongDan(item);
                list.Add(menu);
                GetLoopItemSubItems(ref list, listMenu, menu);
            }
            return list;
        }
        
        
        private async Task<List<MenuCongDan>> UpdateMenu(List<MenuCongDan> list , MenuCongDan menu )
        {
            int index = list.FindIndex(x => x.Id == menu.Id);
            list[index] = menu;
            return list;
        }


        
        private List<MenuTreeVMCongDan> GetLoopItemSubItems(ref List<NavMenuVMCongDan> listMenuVM, List<MenuCongDan> listMenu, NavMenuVMCongDan target)
        {
            var parents = listMenu.Where(x => x.ParentId == target.Id && !x.IsDeleted).OrderBy(x=>x.Sort).ToList();
            if (parents.Count == 0)
                return null;
            List<NavMenuVMCongDan> list = new List<NavMenuVMCongDan>();
            foreach (var item in parents)
            {
                if (target.Children == default)
                    target.Children = new List<NavMenuVMCongDan>();
                if (item != null && item.Id != null)
                {
                    NavMenuVMCongDan donVi = new NavMenuVMCongDan(item);
                    item.IsChecked = true;
                  
                    UpdateMenu(listMenu, item);
                    target.Children.Add(donVi);
                    GetLoopItemSubItems(ref list, listMenu, donVi);  
                }
             
            }
            return null;
        }
        
        
        public async Task<dynamic> GetTreeFlatten()
        {
            List<MenuTreeVMCongDanGetAll> list = new List<MenuTreeVMCongDanGetAll>();
         
            var listDonVi = await _context.MENUCONGDAN.Find(x  => !x.IsDeleted).ToListAsync();
            var parents = listDonVi.Where(x => x.ParentId == null || x.ParentId.Equals("")).ToList();
            var listId = new List<String>();
            foreach (var item in parents)
            {

                MenuTreeVMCongDanGetAll donVi = new MenuTreeVMCongDanGetAll(item);
                list.Add(donVi);
                GetLoopItemGetAll(ref list,listDonVi, donVi);
            }
            return list;
        }
        
        
        private List<DonViCongDanTreeVM> GetLoopItemGetAll(ref List<MenuTreeVMCongDanGetAll> list, List<MenuCongDan> items, MenuTreeVMCongDanGetAll target)
        {
            try
            {
                var coquan = items.FindAll((item) => item.ParentId == target.Id).OrderBy(x=>x.Name).ToList();
                if (coquan.Count > 0)
                {
                    foreach (var item in coquan)
                    {
                        MenuTreeVMCongDanGetAll itemDV = new MenuTreeVMCongDanGetAll(item);
                        list.Add(itemDV);
                        GetLoopItemGetAll(ref list, items, itemDV);
                    }
                }

                return null;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException()
                    .WithCode(DefaultCode.EXCEPTION)
                    .WithMessage(e.ResultString);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("is not a valid 24 digit hex string."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                }
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(ex.Message);
            }
            return null;
        }

        public async Task<bool> DeletedWithUser(IdFromBodyModel fromBodyModel)
        {
            try
            {
                if (fromBodyModel == null)
                {
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_FIELDS_NOT_INCORRECT);
                }

                var filter = Builders<MenuCongDan>.Filter.Eq("IsDeleted", false);
                filter = Builders<MenuCongDan>.Filter.And(filter, Builders<MenuCongDan>.Filter.Eq("Id", fromBodyModel.Id));

                dynamic data = await _context.MENUCONGDAN.Find(filter).FirstOrDefaultAsync();
                if (data == null)
                {
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);
                }

                var currentUser = GetCurrentUser();

                if (currentUser != null)
                {
                    if (currentUser.UserName == "superadmin")
                    {

                        var deletedMenuShortList = _context.MODELCONTENT
                            .Find(x => !x.IsDeleted && x.Menu.Any(menu => menu.Id == fromBodyModel.Id))
                            .FirstOrDefault()?.Menu;

                        if (deletedMenuShortList == null)
                        {

                            var result = await _context.MENUCONGDAN.DeleteOneAsync(filter);
                            if (!result.IsAcknowledged || result.DeletedCount <= 0)
                            {
                                throw new ResponseMessageException().WithException(DefaultCode.DELETE_FAILURE);
                            }
                        }
                        else
                        {
                            throw new ResponseMessageException().WithException(DefaultCode.DELETE_DATA_BEFORE_DELETING_MENU);
                        }
                    }
                    else
                    {
                        var restrictedMenuIds = new List<string>
                            {
                                _settings.menuHoiDap,
                                _settings.menuThongBao,
                                _settings.menuLienHe
                            };

              
                        if (!restrictedMenuIds.Contains(fromBodyModel.Id))
                        {
                            
                            var deletedMenuShortList = _context.MODELCONTENT
                                .Find(x => !x.IsDeleted && x.Menu.Any(menu => menu.Id == fromBodyModel.Id))
                                .FirstOrDefault()?.Menu;

                            if (deletedMenuShortList == null)
                            {
                                
                                var result = await _context.MENUCONGDAN.DeleteOneAsync(filter);
                                if (!result.IsAcknowledged || result.DeletedCount <= 0)
                                {
                                    throw new ResponseMessageException().WithException(DefaultCode.DELETE_FAILURE);
                                }
                            }
                            else
                            {
                                throw new ResponseMessageException().WithException(DefaultCode.DELETE_DATA_BEFORE_DELETING_MENU);
                            }

                        }
                        else
                        {
                           
                            throw new ResponseMessageException().WithException(DefaultCode.ACCOUNT_NOT_AUTHORIZED);
                        }

                    }
                }

                return true;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.ResultString);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("is not a valid 24 digit hex string."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                }
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.Message);
            }
        }







    }
}