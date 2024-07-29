using XoSoKienThiet.FromBodyModels;
using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Models.PagingParam;

namespace XoSoKienThiet.Interfaces.NghiepVu
{
    public interface ILoaiAnhService
    {
        Task<dynamic> Create(LoaiAnh model);
        Task<dynamic> Update(LoaiAnh model);

        Task<PagingModel<LoaiAnh>> GetPagingCore(PagingParam param);


    }
}
