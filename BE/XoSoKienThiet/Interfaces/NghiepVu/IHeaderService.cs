using XoSoKienThiet.Models.NghiepVu;

namespace XoSoKienThiet.Interfaces.NghiepVu
{
    public interface IHeaderService
    {
        Task<dynamic> Create(Header image);
        

        Task<dynamic> GetAll();

    }
}
