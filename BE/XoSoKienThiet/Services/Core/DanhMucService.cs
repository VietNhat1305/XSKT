using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Driver;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Extensions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.ViewModels;

namespace XoSoKienThiet.Services.Core
{
    public class DanhMucService : BaseService, IDanhMucService
    {
        private DataContext _context;
        private BaseMongoDb<Menu, string> BaseMongoDb;
        private List<string> ListActionDefault = new List<string>() { "manage", "create", "update", "delete" };
        public DanhMucService(
            DataContext context,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<Menu, string>(_context.DANHMUC);
        }

        public async Task<dynamic> Create(Menu model)
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
                };
                if (model.ParentId != null)
                {
                    var level = _context.DANHMUC.Find(x => x.Id == model.ParentId).FirstOrDefault();
                    if (level == null)
                        throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage("Đơn vị cha không đúng !");
                    entity.Level = level.Level + 1;
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
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                var entity = _context.DANHMUC.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();
                if (entity == default)
                    throw new ResponseMessageException()
                        .WithException(DefaultCode.DATA_NOT_FOUND);


                /*
                var checkName = _context.MENU.Find(x => !x.IsDeleted &&
                                                        x.Id != model.Id
                                                        && (x.Name == model.Name || x.Resource ==
                                                            FormatterString.ConvertToUnsign(model.Name).Replace(" " ,""))
                ).FirstOrDefault();
                ValidationResult validationResult = new MenuValidation().Validate(model);
                if (!validationResult.IsValid)
                    throw new ResponseMessageException().WithValidationResult(validationResult);
                    */


                /*if (checkName != default)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_EXISTED);*/
                entity.Name = model.Name;
                entity.Path = model.Path;
                entity.Icon = model.Icon;
                entity.ListAction = model.ListAction;
                entity.ParentId = model.ParentId;
                entity.Level = model.Level;
                entity.Sort = model.Sort;
                entity.ModifiedAt = DateTime.Now;
                entity.SetListAction(entity.Name);

                if (model.ParentId != null)
                {
                    var menu = _context.DANHMUC.Find(x => x.Id == model.ParentId).FirstOrDefault();
                    if (menu == null)
                        throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage("Đơn vị cha không đúng !");
                    entity.Level = menu.Level + 1;
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


        #endregion






        public async Task<List<MenuTreeVM>> GetTreeList()
        {
            var listDonVi = await _context.DANHMUC.Find(_ => _.IsDeleted != true).SortBy(donVi => donVi.Level).ToListAsync();
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
        private List<MenuTreeVM> GetLoopItem(ref List<MenuTreeVM> list, List<Menu> items, MenuTreeVM target)
        {

            var menu = items.Where(x => x.ParentId == target.Id).OrderBy(x => x.Sort).ToList();
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




        public async Task<List<NavMenuVM>> GetTreeListMenuForCurrentUser(List<Menu> listMenu)
        {
            var parents = listMenu.Where(x => x.ParentId == null).OrderBy(x => x.Sort).ToList();
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

            var listMenu = await _context.DANHMUC.Find(_ => _.IsDeleted != true).SortBy(donVi => donVi.Level).ToListAsync();
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


        private async Task<List<Menu>> UpdateMenu(List<Menu> list, Menu menu)
        {

            int index = list.FindIndex(x => x.Id == menu.Id);
            list[index] = menu;
            return list;
        }



        private List<MenuTreeVM> GetLoopItemSubItems(ref List<NavMenuVM> listMenuVM, List<Menu> listMenu, NavMenuVM target)
        {
            var parents = listMenu.Where(x => x.ParentId == target.Id && !x.IsDeleted).OrderBy(x => x.Sort).ToList();
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

            var listDonVi = await _context.DANHMUC.Find(x => !x.IsDeleted).ToListAsync();
            var parents = listDonVi.Where(x => x.ParentId == null || x.ParentId.Equals("")).ToList();
            var listId = new List<String>();
            foreach (var item in parents)
            {

                MenuTreeVMGetAll donVi = new MenuTreeVMGetAll(item);
                list.Add(donVi);
                GetLoopItemGetAll(ref list, listDonVi, donVi);
            }
            return list;
        }


        private List<DonViTreeVM> GetLoopItemGetAll(ref List<MenuTreeVMGetAll> list, List<Menu> items, MenuTreeVMGetAll target)
        {
            try
            {
                var coquan = items.FindAll((item) => item.ParentId == target.Id).OrderBy(x => x.Name).ToList();
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