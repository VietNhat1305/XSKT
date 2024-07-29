using XoSoKienThiet.FromBodyModels;
using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Models.PagingParam;

namespace XoSoKienThiet.Interfaces.NghiepVu
{
    public interface IThuVienAnhService
    {
        Task<dynamic> Create(ThuVienAnh files);
        Task<dynamic> Update(ThuVienAnh files);
        Task<dynamic> Tinkhac(PagingParam pagingParam);
        Task<dynamic> GetByIdPhanLoai(string id);

    }
}
