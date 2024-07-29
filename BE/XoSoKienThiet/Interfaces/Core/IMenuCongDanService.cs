using XoSoKienThiet.FromBodyModels;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.ViewModels;

namespace XoSoKienThiet.Interfaces.Core
{
    public interface IMenuCongDanService
    {
        Task<List<MenuTreeVMCongDan>> GetTreeList();
        
        Task<List<MenuTreeVMCongDan>> GetTreeListAD();
        
        
        
        Task<List<MenuTreeVMCongDan>> GetTreeListTBV();
        
        Task<dynamic> GetTreeFlatten();
        
        
        Task<dynamic> Create(MenuCongDan model);
        Task<dynamic> Update(MenuCongDan model);


        
        
        Task<List<MenuTreeVMCongDan>> GetDanhMuc();
        
        
        
        
        Task<List<NavMenuVMCongDan>> GetTreeListMenuForCurrentUser(List<MenuCongDan> listMenu);


        Task<List<NavMenuVMCongDan>> GetTreeListMenuAll();
        Task<bool> DeletedWithUser(IdFromBodyModel fromBodyModel);
    }
}