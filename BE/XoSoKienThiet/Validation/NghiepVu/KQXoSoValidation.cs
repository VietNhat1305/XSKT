using XoSoKienThiet.Models.NghiepVu;
using XoSoKienThiet.Constants;
using System.Globalization;
using FluentValidation;


    public class KQXoSoValidation : AbstractValidator<KetQuaXS>
    {
        public KQXoSoValidation()
        {
            RuleFor(model => model.Date)      
            .NotNull().WithMessage("Ngày đăng kết quả không được bỏ trống.")
            .OverridePropertyName(x => x.Date);

            RuleFor(model => model.Giai8)
            .Must((model, Giai8) =>
            {
                if (Giai8 == null) return true;
                return Validation.Is_Number(Giai8, Giai8.Length);
            }).WithMessage("Kết quả giải tám không đúng định dạng.")
            .Must(model => model != null && model.Length == 2)
                .WithMessage("Giải tám có 2 kí tự.")
            .OverridePropertyName(x => x.Giai8);

            RuleFor(model => model.Giai7)
            .Must((model, Giai7) =>
            {
                if (Giai7 == null) return true;
                return Validation.Is_Number(Giai7, Giai7.Length);
            }).WithMessage("Kết quả giải bảy không đúng định dạng.")
            .Must(model => model != null && model.Length == 3)
                .WithMessage("Giải bảy có 3 kí tự.")
            .OverridePropertyName(x => x.Giai7);

            RuleFor(model => model.Giai6)
            .Must((model, Giai6) =>
            {
                if (Giai6 == null) return true;
                foreach (var item in Giai6)
                {
                    if (!item.Equals(""))
                    {
                        if (!Validation.Is_Number(item, item.Length))
                        {
                            return false;
                        }
                        if (item.Length != 4)
                        {
                            return false;
                        }
                    }                    
                }
                return true;
            }).WithMessage("Kết quả giải sáu không đúng định dạng hoặc không đủ 4 kí tự")
            .OverridePropertyName(x => x.Giai6);

            RuleFor(model => model.Giai5)
            .Must((model, Giai5) =>
            {
                if (Giai5 == null) return true;
                return Validation.Is_Number(Giai5, Giai5.Length);
            }).WithMessage("Kết quả giải năm không đúng định dạng.")
            .Must(model => model != null && model.Length == 4)
            .WithMessage("Giải năm có 4 kí tự.")
            .OverridePropertyName(x => x.Giai5);

            RuleFor(model => model.Giai4)
            .Must((model, Giai4) =>
            {
                if (Giai4 == null) return true;
                foreach (var item in Giai4)
                {
                    if (!item.Equals(""))
                    {
                        if (!Validation.Is_Number(item, item.Length))
                        {
                            return false;
                        }
                        if (item.Length != 5)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }).WithMessage("Kết quả giải tư không đúng định dạng hoặc không đủ 5 kí tự")
            .OverridePropertyName(x => x.Giai4);

            RuleFor(model => model.Giai3)
            .Must((model, Giai3) =>
            {
                if (Giai3 == null) return true;
                foreach (var item in Giai3)
                {
                    if (!item.Equals(""))
                    {
                        if (!Validation.Is_Number(item, item.Length))
                        {
                            return false;
                        }
                        if (item.Length != 5)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }).WithMessage("Kết quả giải ba không đúng định dạng hoặc không đủ 5 kí tự.")
            .OverridePropertyName(x => x.Giai3);

            RuleFor(model => model.Giai2)
            .Must((model, Giai2) =>
            {
                if (Giai2 == null) return true;
                return Validation.Is_Number(Giai2, Giai2.Length);
            }).WithMessage("Kết quả giải nhì không đúng định dạng.")
            .Must(model => model != null && model.Length == 5)
            .WithMessage("Giải nhì có 5 kí tự.")
            .OverridePropertyName(x => x.Giai2);

            RuleFor(model => model.Giai1)
            .Must((model, Giai1) =>
            {
                if (Giai1 == null) return true;
                return Validation.Is_Number(Giai1, Giai1.Length);
            }).WithMessage("Kết quả giải nhất không đúng định dạng.")
            .Must(model => model != null && model.Length == 5)
            .WithMessage("Giải nhất có 5 kí tự.")
            .OverridePropertyName(x => x.Giai1);

            RuleFor(model => model.GiaiDB)
            .Must((model, GiaiDB) =>
            {
                if (GiaiDB == null) return true;
                return Validation.Is_Number(GiaiDB, GiaiDB.Length);
            }).WithMessage("Kết quả giải đặc biệt không đúng định dạng.")
            .Must(model => model != null && model.Length == 6)
            .WithMessage("Giải đặc biệt có 6 kí tự.")
            .OverridePropertyName(x => x.GiaiDB);

    }
}

