using XoSoKienThiet.Models.NghiepVu;

namespace XoSoKienThiet.Interfaces.NghiepVu
{
    
    public interface ILienHeService
    {
        Task<dynamic> Create(LienHeModel model);
        Task<dynamic> GetAll();

    }
}
