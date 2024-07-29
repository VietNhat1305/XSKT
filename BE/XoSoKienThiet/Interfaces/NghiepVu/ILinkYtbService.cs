using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Models.PagingParam;

namespace XoSoKienThiet.Interfaces.NghiepVu
{
    public interface ILinkYtbService
    {
        Task<dynamic> Create(LinkYtb files);
        Task<dynamic> Update(LinkYtb files);
        Task<dynamic> Tinkhac(PagingParam pagingParam);

        //Task<dynamic> GetAll();

    }
}
