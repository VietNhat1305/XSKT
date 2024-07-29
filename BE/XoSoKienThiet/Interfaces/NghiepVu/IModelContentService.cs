using XoSoKienThiet.FromBodyModels;
using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Models.PagingParam;

namespace XoSoKienThiet.Interfaces.NghiepVu
{
    
    public interface IModelContentService
    {
        Task<dynamic> Create(ModelContent model);
        Task<dynamic> Update(ModelContent model);
        Task<dynamic> GetByMenuId(ContentPagingParam pagingParam);

        Task<dynamic> Tinkhac(ContentPagingParam pagingParam);
        
        
        Task<dynamic> GetPaging(ModelContentPagingParam  param);
        
        
        Task<dynamic> GetPagingHoiDap(ModelContentPagingParam  param);
        Task<dynamic> GetPagingHoiDapCongDan(ModelContentPagingParam  param);
        
        Task<dynamic> CreateHoiDap(ModelContent model);
        Task<dynamic> UpdateHoiDap(ModelContent model);
        
        
        
        
        Task<dynamic> GetTinTrangChu();
        
        
    }
}
