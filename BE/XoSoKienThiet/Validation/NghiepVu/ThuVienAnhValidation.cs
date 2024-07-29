using FluentValidation;
using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Constants;

public class ThuVienAnhValidation : AbstractValidator<ThuVienAnh>
{
    public ThuVienAnhValidation()
    {


        //RuleFor(model => model.File)
        //    .Must((model, files) => files != null).WithMessage("Tệp tin không được trống.");

        //RuleFor(model => model.LoaiAnhId)
        //    .NotEmpty().WithMessage("Loại ảnh không được bỏ trống.")
        //    .NotNull().WithMessage("Loại ảnh không được null.")
        //    .OverridePropertyName(x => x.LoaiAnhId);
    }
}

