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

namespace XoSoKienThiet.Services.Core
{
    public class MenuService : BaseService, IMenuService
    {
        private DataContext _context;
        private BaseMongoDb<Menu, string> BaseMongoDb;
        private BaseMongoDb<UnitRole, string> BaseMongoDbUnitRole;
        private List<string> ListActionDefault = new List<string>() {"manage", "create", "update", "delete"};
        public MenuService(
            DataContext context,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<Menu, string>(_context.MENU);
            BaseMongoDbUnitRole= new BaseMongoDb<UnitRole, string>(_context.UNIT_ROLE); 
        }

      

        public async Task<dynamic> Create(Menu model)
        {
            try
            {
                
            
            if (model == default)
                throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
            
            var menu = _context.MENU.Find(x => !x.IsDeleted &&  (x.Name == model.Name || x.UnsignedName == FormatterString.
                ConvertToUnsign(model.Name))).FirstOrDefault();
            if (menu != default)
            {
                throw new ResponseMessageException()
                    .WithException(DefaultCode.DATA_EXISTED);
            }
            ValidationResult validationResult = new MenuValidation().Validate(model);
            if (!validationResult.IsValid)         
                throw new ResponseMessageException().WithValidationResult(validationResult);     
            var entity = new Menu
            {
                Id = BsonObjectId.GenerateNewId().ToString(),
                Name = model.Name,
                Path = model.Path,
                Icon = model.Icon,
                ParentId = model.ParentId,
                Sort = model.Sort,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                isPrivate = model.isPrivate,
                IsAnhDaiDien = model.IsAnhDaiDien,
                IsTepTin = model.IsTepTin,
                IsKyHieu = model.IsKyHieu,
                IsMoTa = model.IsMoTa,
                IsNgayKy = model.IsNgayKy,
                IsNgayXuatBan  = model.IsNgayXuatBan,
                IsNoiDung = model.IsNoiDung,
                IsTieuDe=model.IsTieuDe,
                IsTrichYeu  = model.IsTrichYeu,
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
                IsDanhMuc = model.IsDanhMuc,
                IsMenu = model.IsMenu,
                IsTrangChu = model.IsTrangChu,
            };
            if (model.ParentId != null)
            {
                var level = _context.MENU.Find(x => x.Id == model.ParentId).FirstOrDefault();
                if (level == null)
                    throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage("Đơn vị cha không đúng !");
                entity.Level= level.Level + 1; 
            }
            entity.SetListAction(entity.Name);
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

           public async Task<dynamic> Update(Menu model)
        {
            try{
            if (model == default)
                throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
            var entity = _context.MENU.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();
            var oldResource = entity.UnsignedName; 
            if (entity == default)
                throw new ResponseMessageException()
                    .WithException(DefaultCode.DATA_NOT_FOUND);

            var checkName = _context.MENU.Find(x => !x.IsDeleted &&
                                                    x.Id != model.Id
                                                    && (x.Name == model.Name || x.UnsignedName ==
                                                        FormatterString.ConvertToUnsign(model.Name).Replace(" " ,""))
            ).FirstOrDefault();
            ValidationResult validationResult = new MenuValidation().Validate(model);
            if (!validationResult.IsValid)
                throw new ResponseMessageException().WithValidationResult(validationResult);

            if (checkName != default)
                throw new ResponseMessageException().WithException(DefaultCode.DATA_EXISTED);
            entity.Name = model.Name;
            entity.Path = model.Path;
            entity.Icon = model.Icon;
            entity.ListAction = model.ListAction;
            entity.ParentId = model.ParentId;
            entity.Level = model.Level;
            entity.Sort = model.Sort;
            entity.ModifiedAt = DateTime.Now;
            entity.SetListAction(entity.Name);
            entity.isPrivate = model.isPrivate;
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
            
            if (model.ParentId != null)
            {
                var menu = _context.MENU.Find(x => x.Id == model.ParentId).FirstOrDefault();
                if (menu == null)
                    throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage("Đơn vị cha không đúng !");
                entity.Level= menu.Level + 1; 
            }
            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
                throw new ResponseMessageException().WithException(DefaultCode.UPDATE_FAILURE);


            if (!oldResource.Equals(entity.UnsignedName))
            {
                UpdateUnitRole(oldResource, entity.UnsignedName, entity.Id);
            }
            
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
           

           private async Task<dynamic> UpdateUnitRole(string actionOld , string actionNew , string id)
           {
               var unitRoles = _context.UNIT_ROLE.Find(x => x.ListMenu.Contains(actionOld)).ToList();
               foreach (var item in unitRoles)
               {
                   var listAction = item.ListAction.FindAll(x => x.Contains(id));
                   item.ListMenu.RemoveAll(x => x.Contains(actionOld));
                   item.ListAction.RemoveAll(x => x.Contains(id));
                   foreach (var action in listAction)
                   {
                       var list = action.Split("-");
                       item.ListAction.Add(list[0] + "-" + actionNew + "-" + id);
                   }
                   item.ListMenu.Add(actionNew);
                   item.ModifiedAt = DateTime.Now;
                   item.ModifiedBy = CurrentUserName;
                   var result = await BaseMongoDbUnitRole.UpdateAsync(item);
                   if (!result.Success)
                       return false;
               }
               return true;
           }
        #endregion
     
        
        
        
        
        
        public async Task<List<MenuTreeVM>> GetTreeList()
        {
            var listDonVi = await _context.MENU.Find(_ => _.IsDeleted != true).SortBy(donVi => donVi.Sort).ToListAsync();
            var parents = listDonVi.Where(x => x.ParentId == null || x.ParentId.Equals("")).ToList();
            List<MenuTreeVM> list = new List<MenuTreeVM>();
            foreach (var item in parents)
            {
                MenuTreeVM donVi = new MenuTreeVM(item);
                list.Add(donVi);
                GetLoopItem(ref list, listDonVi, donVi);
            }
            return list;
        }
        
        public async Task<List<MenuTreeVM>> GetTreeListNoiBo()
        {
            var listDonVi = await _context.MENU.Find(_ => _.IsDeleted != true && _.isPrivate==true).SortBy(donVi => donVi.Sort).ToListAsync();
            var parents = listDonVi.Where(x => x.ParentId == null || x.ParentId.Equals("")).ToList();
            List<MenuTreeVM> list = new List<MenuTreeVM>();
            foreach (var item in parents)
            {
                MenuTreeVM donVi = new MenuTreeVM(item);
                
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
        
        public async Task<List<MenuTreeAndActionVM>> GetTreeListAndAction()
        {
            var listDonVi = await _context.MENU.Find(_ => _.IsDeleted != true).SortBy(donVi => donVi.Sort).ToListAsync();
            var parents = listDonVi.Where(x => x.ParentId == null || x.ParentId.Equals("")).ToList();
            List<MenuTreeAndActionVM> list = new List<MenuTreeAndActionVM>();
            foreach (var item in parents)
            {
                MenuTreeAndActionVM donVi = new MenuTreeAndActionVM(item);
                list.Add(donVi);
                GetLoopItemAC(ref list, listDonVi, donVi);
            }
            return list;
        }
        private List<MenuTreeVM> GetLoopItem(ref List<MenuTreeVM> list, List<Menu> items, MenuTreeVM target)
        {
            
            var menu =  items.Where(x=>x.ParentId == target.Id).OrderBy(x=>x.Sort).ToList();
            if (menu.Count > 0)
            {
                target.Children = new List<MenuTreeVM>();
                foreach (var item in menu)
                {
                    MenuTreeVM itemDV = new MenuTreeVM(item);
                    target.Children.Add(itemDV);
                    // target.SubItems.Add(itemDV);
                    GetLoopItem(ref list, items, itemDV);
                }
            }
            return null;
        }
        
        
        
        
        private List<MenuTreeAndActionVM> GetLoopItemAC(ref List<MenuTreeAndActionVM> list, List<Menu> items, MenuTreeAndActionVM target)
        {
            
            var menu =  items.Where(x=>x.ParentId == target.Id).OrderBy(x=>x.Sort).ToList();
            if (menu.Count > 0)
            {
                target.Children = new List<MenuTreeAndActionVM>();
                foreach (var item in menu)
                {
                    MenuTreeAndActionVM itemDV = new MenuTreeAndActionVM(item);
                    target.Children.Add(itemDV);
                    // target.SubItems.Add(itemDV);
                    GetLoopItemAC(ref list, items, itemDV);
                }
            }
            return null;
        }





        public async Task<dynamic> AddAC(MenuList model)
        {
            if (model == null || _context.MENU == null)
                throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

            var entity = await _context.MENU.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefaultAsync();

            if (entity == null)
                throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

            var actionName = FormatterString.ConvertToUnsign(model.ListAction).Replace(" ", "").ToLower() + "-" + FormatterString.ConvertToUnsign(entity.Name).Replace(" ", "");

            if (entity.ListAction.Contains(actionName))
                throw new ResponseMessageException().WithException(DefaultCode.DATA_EXISTED);

            
            entity.ListAction.Add(actionName);
            var filter = Builders<Menu>.Filter.Where(x => x.Id == model.Id);
            var update = Builders<Menu>.Update
                .Set(x => x.ListAction, entity.ListAction);
                
                
            var result = _context.MENU.UpdateOneAsync(filter, update);
            if (result.Result.MatchedCount == 0)
            {
                throw new ResponseMessageException().WithException(DefaultCode.UPDATE_FAILURE);
            }
                
            return entity;
        }

        public async Task<dynamic> DeleteAc(MenuList model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                
                var entity = await _context.MENU
                    .Find(x => !x.IsDeleted && x.Id == model.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

                entity.ListAction.Remove(model.ListAction.Replace(" ", ""));

                var filter = Builders<Menu>.Filter.Where(x => x.Id == model.Id);
                var update = Builders<Menu>.Update
                    .Set(x => x.ListAction, entity.ListAction);
                
                
                var result = _context.MENU.UpdateOneAsync(filter, update);
                if (result.Result.MatchedCount == 0)
                {
                    throw new ResponseMessageException().WithException(DefaultCode.UPDATE_FAILURE);
                }
                
                return entity;



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
           
        }
        
        
        public async Task<List<NavMenuVM>> GetTreeListMenuForCurrentUser(List<Menu> listMenu)
        {
            var parents = listMenu.Where(x => x.ParentId == null).OrderBy(x=>x.Sort).ToList();
            List<NavMenuVM> list = new List<NavMenuVM>();
            foreach (var item in parents) 
            {
                if (item != null && item.Id != null)
                {
                    NavMenuVM menu = new NavMenuVM(item);
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
                    NavMenuVM menu = new NavMenuVM(item);
                    list.Add(menu);
                    item.IsChecked = true;
                    UpdateMenu(listMenu, item);
                    GetLoopItemSubItems(ref list, listMenu, menu);
                        
                }
              
            }
            return list;
        }
        
        
        
        
        public async Task<List<NavMenuVM>> GetTreeListMenuAll()
        {

            var listMenu = await _context.MENU.Find(_ => _.IsDeleted != true).SortBy(donVi => donVi.Level).ToListAsync();
            var parents = listMenu.Where(x => x.ParentId == null || x.ParentId.Equals("")).ToList();
            
            List<NavMenuVM> list = new List<NavMenuVM>();
            foreach (var item in parents)
            {
                NavMenuVM menu = new NavMenuVM(item);
                list.Add(menu);
                GetLoopItemSubItems(ref list, listMenu, menu);
            }
            return list;
        }
        
        
        private async Task<List<Menu>> UpdateMenu(List<Menu> list , Menu menu )
        {

            int index = list.FindIndex(x => x.Id == menu.Id);
            list[index] = menu;
            return list;
        }


        
        private List<MenuTreeVM> GetLoopItemSubItems(ref List<NavMenuVM> listMenuVM, List<Menu> listMenu, NavMenuVM target)
        {
            var parents = listMenu.Where(x => x.ParentId == target.Id).OrderBy(x=>x.Sort).ToList();
            if (parents.Count == 0)
                return null;
            List<NavMenuVM> list = new List<NavMenuVM>();
            foreach (var item in parents)
            {
                if (target.Children == default)
                    target.Children = new List<NavMenuVM>();
                if (item != null && item.Id != null)
                {
                    NavMenuVM donVi = new NavMenuVM(item);
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
            List<MenuTreeVMGetAll> list = new List<MenuTreeVMGetAll>();
         
            var listDonVi = await _context.MENU.Find(x  => !x.IsDeleted).ToListAsync();
            var parents = listDonVi.Where(x => x.ParentId == null || x.ParentId.Equals("")).ToList();
            var listId = new List<String>();
            foreach (var item in parents)
            {
                    
                MenuTreeVMGetAll donVi = new MenuTreeVMGetAll(item);
                list.Add(donVi);
                GetLoopItemGetAll(ref list,listDonVi, donVi);
            }
            return list;
        }
        
        
        private List<DonViTreeVM> GetLoopItemGetAll(ref List<MenuTreeVMGetAll> list, List<Menu> items, MenuTreeVMGetAll target)
        {
            try
            {
                var coquan = items.FindAll((item) => item.ParentId == target.Id).OrderBy(x=>x.Name).ToList();
                if (coquan.Count > 0)
                {
                    foreach (var item in coquan)
                    {
                        MenuTreeVMGetAll itemDV = new MenuTreeVMGetAll(item);
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

        
        
        
        
        
        




    }
}