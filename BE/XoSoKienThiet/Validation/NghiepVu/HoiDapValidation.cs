using FluentValidation;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Models.NghiepVu;

namespace XoSoKienThiet.Validation.NghiepVu
{
    public class HoiDapValidation : AbstractValidator<ModelContent>
    {
        public HoiDapValidation()
        {
            RuleFor(model => model.MoTa)
           .NotEmpty().WithMessage("Nội dung câu hỏi không được bỏ trống.")
           .NotNull().WithMessage("Nội dung câu hỏi không được null.")
           .OverridePropertyName(x => x.MoTa);

        }
    }
}
