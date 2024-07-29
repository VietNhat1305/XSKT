using XoSoKienThiet.Models.NghiepVu;

namespace XoSoKienThiet.Interfaces.NghiepVu
{
    public interface IDiemLoToService
    {
        Task<dynamic> Create(DiemLoTo files);
        Task<dynamic> Update(DiemLoTo files);
        

        //Task<dynamic> GetAll();

    }
}
