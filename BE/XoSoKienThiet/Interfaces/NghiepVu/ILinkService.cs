using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Models.PagingParam;

namespace XoSoKienThiet.Interfaces.NghiepVu
{
    public interface ILinkService
    {
        Task<dynamic> Create(LinkModel files);
        Task<dynamic> Update(LinkModel files);
        Task<dynamic> Tinkhac(PagingParam pagingParam);

        //Task<dynamic> GetAll();

    }
}
