using XoSoKienThiet.Models.NghiepVu;

namespace XoSoKienThiet.Interfaces.NghiepVu
{
    public interface IEditImgService
    {
        Task<dynamic> Create(KetQuaXS model);
        Task<dynamic> Update(KetQuaXS model);
        //Task<dynamic> Get(DateTime date);
        Task<dynamic> GetModel(string ketqua, DateTime date);
        Task<dynamic> GetByDate(DateTime date);
    }
}
