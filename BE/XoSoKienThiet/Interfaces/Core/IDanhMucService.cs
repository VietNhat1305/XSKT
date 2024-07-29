using XoSoKienThiet.Models.Core;
using XoSoKienThiet.ViewModels;

namespace XoSoKienThiet.Interfaces.Core
{
    public interface IDanhMucService
    {
        Task<List<MenuTreeVM>> GetTreeList();

        Task<dynamic> GetTreeFlatten();


        Task<dynamic> Create(Menu model);
        Task<dynamic> Update(Menu model);


        Task<List<NavMenuVM>> GetTreeListMenuForCurrentUser(List<Menu> listMenu);


        Task<List<NavMenuVM>> GetTreeListMenuAll();
    }
}
