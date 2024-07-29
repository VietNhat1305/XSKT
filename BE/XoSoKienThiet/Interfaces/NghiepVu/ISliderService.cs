using XoSoKienThiet.Models.NghiepVu;

namespace XoSoKienThiet.Interfaces.NghiepVu
{
    
    public interface ISliderService
    {
        Task<dynamic> Create(Slider files);


        Task<dynamic> GetAll();

    }
}
