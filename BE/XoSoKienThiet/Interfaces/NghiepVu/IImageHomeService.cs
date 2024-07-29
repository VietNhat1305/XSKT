using XoSoKienThiet.Models.NghiepVu;

namespace XoSoKienThiet.Interfaces.NghiepVu
{
    public interface IImageHomeService
    {
        Task<dynamic> Create(ImageHome files);
        Task<dynamic> Update(ImageHome files);
        

        //Task<dynamic> GetAll();

    }
}
