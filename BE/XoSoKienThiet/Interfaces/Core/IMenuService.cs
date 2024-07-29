using XoSoKienThiet.FromBodyModels;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.ViewModels;

namespace XoSoKienThiet.Interfaces.Core
{
    public interface IMenuService
    {
        Task<List<MenuTreeVM>> GetTreeList();
        
        
        Task<List<MenuTreeAndActionVM>> GetTreeListAndAction();
        
        Task<dynamic> GetTreeFlatten();
        
        
        
        
        Task<dynamic> Create(Menu model);
        Task<dynamic> Update(Menu model);
        Task<dynamic> AddAC(MenuList model);
        Task<dynamic>  DeleteAc(MenuList model);
        
        
        Task<List<NavMenuVM>> GetTreeListMenuForCurrentUser(List<Menu> listMenu);


        Task<List<NavMenuVM>> GetTreeListMenuAll(); 
        
        Task<List<MenuTreeVM>> GetTreeListNoiBo();
    }
}