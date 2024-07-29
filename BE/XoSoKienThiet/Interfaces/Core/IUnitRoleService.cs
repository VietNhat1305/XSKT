using XoSoKienThiet.FromBodyModels;
using XoSoKienThiet.Models.Core;

namespace XoSoKienThiet.Interfaces.Core
{
    public interface IUnitRoleService
    {
        Task<dynamic> Create(UnitRole model);
        Task<dynamic> Update(UnitRole model);
        
        
        Task<dynamic> UpdateAction(UnitRole model);
        




    }
}