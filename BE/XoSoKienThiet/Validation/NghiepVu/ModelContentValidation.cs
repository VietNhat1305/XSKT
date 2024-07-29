using FluentValidation;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.Models.NghiepVu;
using ValidationException = FluentValidation.ValidationException;

public class ModelContentValidation : AbstractValidator<ModelContent>
{
    private readonly DataContext _context;

    public ModelContentValidation(DataContext context)
    {
        _context = context;

        RuleFor(model => model.Menu)
            .Must((model, menu) => IsMenuValid(model))
            .WithMessage("Menu sai định dạng !");

        RuleFor(model => model.Name)
            .NotEmpty().WithMessage("Tiêu đề không được trống.");

        RuleFor(model => model.MoTa)
            .NotEmpty().WithMessage("Mô tả không được trống.");

        RuleFor(model => model.NgayKy)
            .NotEmpty().WithMessage("Ngày ký không được trống.");

        RuleFor(model => model.NgayXuatBan)
            .NotEmpty().WithMessage("Ngày xuất bản không được trống.");

        RuleFor(model => model.FileImage)
            .NotEmpty().WithMessage("Ảnh đại diện không được trống.");

        RuleFor(model => model.SoKiHieu)
            .NotEmpty().WithMessage("Số ký hiệu không được trống.");

        RuleFor(model => model.NoiDung)
            .NotEmpty().WithMessage("Nội dung không được trống.");

        RuleFor(model => model.Files)
            .Must((model, files) => files != null && files.Count > 0).WithMessage("Tệp tin không được trống.");

        RuleFor(model => model.TrichYeu)
            .NotEmpty().WithMessage("Trích yếu không được trống.");
    }

    private bool IsMenuValid(ModelContent model)
    {
        if (Validation.IsCommonMenuCongDanShortVaild(model.Menu))
        {
            foreach (var item in model.Menu)
            {
                var menuEntity = _context.MENUCONGDAN.Find(m => m.Id == item.Id && !m.IsDeleted)?.FirstOrDefault();
                try
                {
                    if (menuEntity == null)
                    {
                        AddValidationError("Menu không hợp lệ.");
                        return false;
                    }
                }
                catch (ValidationException ex)
                {
                   
                    Console.WriteLine($"Validation error: {ex.Message}");
                }
            }
            return true;
        }
        return false;
    }

    private List<string> validationErrors = new List<string>();

    private void AddValidationError(string errorMessage)
    {
        validationErrors.Add(errorMessage);
    }
}
