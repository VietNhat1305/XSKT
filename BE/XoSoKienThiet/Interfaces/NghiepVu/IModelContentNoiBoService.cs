using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Models.PagingParam;

namespace XoSoKienThiet.Interfaces.NghiepVu;

public interface IModelContentNoiBoService
{
    Task<dynamic> Create(modelContentNoiBo model);
    Task<dynamic> GetByMenuId(ContentPagingParam pagingParam);
    Task<dynamic> Update(modelContentNoiBo model);
  
    
    Task<dynamic> GetPaging(ModelContentPagingParam  param);
    
}