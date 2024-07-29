using XoSoKienThiet.Models.NghiepVu;

namespace XoSoKienThiet.Interfaces.NghiepVu
{
    
    public interface ISuKienService
    {
        Task<dynamic> Create(SuKien model);
        Task<dynamic> Update(SuKien model);

    }
}
