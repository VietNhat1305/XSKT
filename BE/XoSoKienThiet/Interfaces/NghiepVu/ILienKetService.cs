using XoSoKienThiet.Models.NghiepVu;

namespace XoSoKienThiet.Interfaces.NghiepVu
{
    public interface ILienKietService
    {
        Task<dynamic> Create(LienKet files);
        Task<dynamic> Update(LienKet files);
        

        //Task<dynamic> GetAll();

    }
}
