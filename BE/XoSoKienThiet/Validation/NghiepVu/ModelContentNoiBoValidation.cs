using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Constants;

using System.Globalization;
using FluentValidation;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.Installers;
using MongoDB.Driver;
using XoSoKienThiet.Models.Appsettings;


public class ModelContentNoiBoValidation : AbstractValidator<modelContentNoiBo>
{
    public ModelContentNoiBoValidation(DataContext _context)
    {
        RuleFor(model => model)
            .NotEmpty().WithMessage("Menu là bắt buộc.")
            .NotNull().WithMessage("Menu không được null.")
           .Must(x =>
           {
               if (Validation.IsCommonMenuCongDan1Vaild(x.Menu))
               {
                   foreach (var item in x.Menu)
                   {
                       var menu = _context.MENU.Find(m => m.Id == item.Id && !m.IsDeleted)?.FirstOrDefault();

                       if (menu == null)
                       {
                           return false;
                       }
                       if (menu.IsTieuDe)
                       {

                           if (x.Name == null || x.Name.Equals(""))
                               // throw new ValidationException("Tiêu đề không được trống.");
                               return false;
                       }
                       if (menu.IsMoTa)
                       {

                           if (x.MoTa == null || x.MoTa.Equals(""))
                               //throw new ValidationException("Mô tả không được trống.");
                               return false;
                       }
                       if (menu.IsNgayKy)
                       {

                           if (x.NgayKy == null || x.NgayKy.Equals(""))
                               // throw new ValidationException("Ngày ký không được trống.");
                               return false;
                       }

                       if (menu.IsNgayXuatBan)
                       {

                           if (x.NgayXuatBan == null || x.NgayXuatBan.Equals(""))
                               // throw new ValidationException("Ngày xuất bản không được trống.");
                               return false;
                       }
                       if (menu.IsAnhDaiDien)
                       {

                           if (x.FileImage == null || x.FileImage.Equals(""))
                               // throw new ValidationException("Ảnh đại diện không được trống.");
                               return false;
                       }
                       if (menu.IsKyHieu)
                       {

                           if (x.SoKiHieu == null || x.SoKiHieu.Equals(""))
                               //throw new ValidationException("Số ký hiệu không được trống.");
                               return false;
                       }
                       if (menu.IsNoiDung)
                       {

                           if (x.NoiDung == null || x.NoiDung.Equals(""))
                               // throw new ValidationException("Nội dung không được trống.");
                               return false;
                       }
                       if (menu.IsTepTin)
                       {

                           if (x.Files.Count == 0 || (x.Files.Count != 0 && x.Files.Equals("")))
                               // throw new ValidationException("Tệp tin không được trống.");
                               return false;
                       }
                       if (menu.IsTrichYeu)
                       {

                           if (x.TrichYeu == null || x.TrichYeu.Equals(""))


                               //throw new ValidationException("Trích yếu không được trống.");
                               return false;

                       }
                   }
                   return true;
               }
               return false;
           })

        .WithMessage("Menu sai định dạng !")
        .OverridePropertyName(x => x.Menu);




    }
}
