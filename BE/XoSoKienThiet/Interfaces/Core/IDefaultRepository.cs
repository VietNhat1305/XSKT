using XoSoKienThiet.FromBodyModels;
using XoSoKienThiet.Models.PagingParam;

namespace XoSoKienThiet.Interfaces.Core
{
    public interface IDefaultReposityService<T> where T : class
    {
        Task<List<dynamic>> GetAll();
        Task<dynamic> GetById(IdFromBodyModel fromBodyModel);
        Task<bool> Delete(IdFromBodyModel fromBodyModel);
        
        Task<bool> Deleted(IdFromBodyModel fromBodyModel);
        Task<dynamic> GetPagingCore(PagingParam pagingParam);
    }
}
