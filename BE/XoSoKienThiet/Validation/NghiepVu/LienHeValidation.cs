using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Constants;
using System.Globalization;
using FluentValidation;


    public class LienHeValidation : AbstractValidator<LienHeModel>
    {
        public LienHeValidation()
        {
            RuleFor(model => model.Name)
           .NotEmpty().WithMessage("Tên không được bỏ trống.")
           .NotNull().WithMessage("Tên không được null.")
           .OverridePropertyName(x => x.Name);

            RuleFor(model => model.DiaChi)
           .NotEmpty().WithMessage("Địa chỉ không được bỏ trống.")
           .NotNull().WithMessage("Địa chỉ không được null.")
           .OverridePropertyName(x => x.DiaChi);

            RuleFor(model => model.SoDienThoai)
            .NotEmpty().WithMessage("Số điện thoại là bắt buộc.")
            .NotNull().WithMessage("Số điện thoại không được null.")
            .OverridePropertyName(x => x.SoDienThoai);

            RuleFor(model => model.Fax)
            .NotEmpty().WithMessage("Số Fax không được bỏ trống.")
            .NotNull().WithMessage("Số Fax không được null.")
            .OverridePropertyName(x => x.Fax);



            RuleFor(model => model.Email)
            .NotEmpty().WithMessage("Email không được bỏ trống.")
            .EmailAddress().WithMessage("Địa chỉ email không hợp lệ.")
            .OverridePropertyName(x => x.Email);

            RuleFor(model => model.VanPhongDaiDien)
            .NotEmpty().WithMessage("Văn phòng đại diệnkhông được bỏ trống.")
            .NotNull().WithMessage("Văn phòng đại diện không được null.")
            .OverridePropertyName(x => x.VanPhongDaiDien);

            RuleFor(model => model.MaSoThue)
            .NotEmpty().WithMessage("Mã số thuế không được bỏ trống.")
            .NotNull().WithMessage("Mã số thuế không được null.")
            .OverridePropertyName(x => x.MaSoThue);

            RuleFor(model => model.NguoiBienTap)
            .NotEmpty().WithMessage("Người biên tập không được bỏ trống.")
            .NotNull().WithMessage("Người biên tập không được null.")
            .OverridePropertyName(x => x.NguoiBienTap);

            /*RuleFor(model => model.Menu)
            .NotEmpty().WithMessage("Menu không được bỏ trống.")
            .NotNull().WithMessage("Menu không được null.")
            .OverridePropertyName(x => x.Menu);*/
        }
    }

