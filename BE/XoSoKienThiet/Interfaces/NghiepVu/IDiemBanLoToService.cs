using XoSoKienThiet.Models.NghiepVu;
namespace XoSoKienThiet.Interfaces.NghiepVu
{
    public interface IDiemBanLoToService
    {
        Task<dynamic> Create(DiemBanLoTo files);
        Task<dynamic> Update(DiemBanLoTo files);
    }
}
