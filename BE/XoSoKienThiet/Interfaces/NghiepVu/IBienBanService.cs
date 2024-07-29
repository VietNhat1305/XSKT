using XoSoKienThiet.FromBodyModels;
using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Models.PagingParam;

namespace XoSoKienThiet.Interfaces.NghiepVu
{
    public interface IBienBanService
    {
        Task<dynamic> Create(BienBan files);
        Task<dynamic> Update(BienBan files);

        Task<dynamic> Tinkhac(PagingParam pagingParam);
        //Task<dynamic> GetAll();

    }
}
