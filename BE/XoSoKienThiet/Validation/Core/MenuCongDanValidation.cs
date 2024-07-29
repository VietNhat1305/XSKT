using FluentValidation;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Models.Core;

public class MenuCongDanValidation : AbstractValidator<MenuCongDan>
{
    public MenuCongDanValidation()
    {
        //RuleFor(model => model.Name)
        //    .NotEmpty().WithMessage("Tên không được để trống.")
        //    .NotNull().WithMessage("Tên không được để null.")
        //    .OverridePropertyName(x => x.Name);


        /*RuleFor(model => model.Path)
            .NotEmpty().WithMessage("Đường dẫn không được bỏ trống.")
            .OverridePropertyName(x => x.Path);*/

        RuleFor(model => model.Icon)
            .NotEmpty().WithMessage("Icon không được để trống.")
            .OverridePropertyName(x => x.Icon);

        RuleFor(model => model.IsKyHieu)
            .NotEmpty()
            .When(model => model.IsKyHieu)
            .WithMessage("Ký hiệu không được để trống.")
            .OverridePropertyName(x => x.IsKyHieu)
            .Must((model, isKyHieu) =>
            {
                if (isKyHieu)
                {
                    
                    model.IsShowKyHieu = true;
                }
                else
                {
                   
                    model.IsShowKyHieu = model.IsShowKyHieu; 
                }
                return true;
            })
            .WithMessage("Nếu ràng buộc ký hiệu chọn, thì hiển thị ký hiệu cũng phải chọn.")
            .OverridePropertyName(x => x.IsShowKyHieu);


        RuleFor(model => model.IsMoTa)
           .NotEmpty()
           .When(model => model.IsMoTa)
           .WithMessage("Mô tả không được để trống.")
           .OverridePropertyName(x => x.IsMoTa)
           .Must((model, IsMoTa) =>
           {
               if (IsMoTa)
               {

                   model.IsShowMoTa =true;
               }
               else
               {

                   model.IsShowMoTa = model.IsShowMoTa;
               }
               return true;
           })
           .WithMessage("Nếu hiển thị mô tả chọn, thì ràng buộc mô tả cũng phải chọn.")
           .OverridePropertyName(x => x.IsShowKyHieu);



        //RuleFor(model => model.IsMoTa)
        //.NotEmpty()
        //.When(model => model.IsMoTa) 
        //.WithMessage("Mô tả không được để trống.")
        //.OverridePropertyName(x => x.IsMoTa);


        RuleFor(model => model.IsTieuDe)

          .NotEmpty()
          .When(model => model.IsTieuDe)
          .WithMessage("Tiêu đề không được để trống.")
           .OverridePropertyName(x => x.IsTieuDe)
       .Must((model, IsTieuDe, IsShowTieuDe) =>
       {

           if (IsTieuDe)
           {

               model.IsShowTieuDe = true;

           }
           else if (model.IsShowTieuDe != true)
           {

               model.IsTieuDe = false;
           }
           else if (model.IsShowTieuDe == true)
           {
               model.IsTieuDe = model.IsTieuDe;
           }


           return true;
       })
           .WithMessage("Nếu ràng buộc tiêu đề chọn, thì hiển thị tiêu đề cũng phải chọn.")
          .OverridePropertyName(x => x.IsShowTieuDe);


        //RuleFor(model => model.IsTieuDe)
        //.NotEmpty()
        //.When(model => model.IsTieuDe)
        //.WithMessage("Tiêu đề không được để trống.")
        //.OverridePropertyName(x => x.IsTieuDe);

        RuleFor(model => model.IsTrichYeu)
            
         .NotEmpty()
         .When(model => model.IsTrichYeu)
         .WithMessage("Trích yếu không được để trống.")
          .OverridePropertyName(x => x.IsTrichYeu)
      .Must((model, IsTrichYeu) =>
      {

          if (IsTrichYeu)
          {

              model.IsShowTrichYeu = true;
          }
          else
          {

              model.IsShowTrichYeu = model.IsShowTrichYeu;
          }
          return true;
      })
         .WithMessage("Nếu ràng buộc trích yếu chọn, thì hiển thị trích yếu cũng phải chọn.")
         .OverridePropertyName(x => x.IsShowTrichYeu);


        //  RuleFor(model => model.IsTrichYeu)
        //.NotEmpty()
        //.When(model => model.IsTrichYeu)
        //.WithMessage("Trích yếu không được để trống.")
        //.OverridePropertyName(x => x.IsTrichYeu);

        RuleFor(model => model.IsNgayXuatBan)
      .NotEmpty()
      .When(model => model.IsNgayXuatBan)
      .WithMessage("Ngày xuất bản không được để trống.")
      .OverridePropertyName(x => x.IsNgayXuatBan)
      .Must((model, IsNgayXuatBan) =>
      {

          if (IsNgayXuatBan)
          {

              model.IsShowNgayXuatBan = true;
          }
          else
          {

              model.IsShowNgayXuatBan = model.IsShowNgayXuatBan;
          }
          return true;
      })
         .WithMessage("Nếu ràng buộc ngày xuất bản chọn, thì hiển thị ngày xuất bản cũng phải chọn.")
         .OverridePropertyName(x => x.IsShowNgayXuatBan);

        //RuleFor(model => model.IsNgayXuatBan)
        //.NotEmpty()
        //.When(model => model.IsNgayXuatBan)
        //.WithMessage("Ngày xuất bản không được để trống.")
        //.OverridePropertyName(x => x.IsNgayXuatBan);

        RuleFor(model => model.IsAnhDaiDien)
      .NotEmpty()
      .When(model => model.IsAnhDaiDien)
      .WithMessage("Ảnh đăng bài không được để trống.")
      .OverridePropertyName(x => x.IsAnhDaiDien)
      .Must((model, IsAnhDaiDien) =>
      {

          if (IsAnhDaiDien)
          {

              model.IsShowAnhDaiDien = true;
          }
          else
          {

              model.IsShowAnhDaiDien = model.IsShowAnhDaiDien;
          }
          return true;
      })
         .WithMessage("Nếu ràng buộc ảnh đăng bài chọn, thì hiển thị ảnh đăng bài cũng phải chọn.")
         .OverridePropertyName(x => x.IsShowAnhDaiDien);


        //RuleFor(model => model.IsAnhDaiDien)
        //.NotEmpty()
        //.When(model => model.IsAnhDaiDien)
        //.WithMessage("Ảnh đại diện không được để trống.")
        //.OverridePropertyName(x => x.IsAnhDaiDien);
        RuleFor(model => model.IsNgayKy)
      .NotEmpty()
      .When(model => model.IsNgayKy)
      .WithMessage("Ngày ký không được để trống.")
      .OverridePropertyName(x => x.IsNgayKy)
      .Must((model, IsNgayKy) =>
      {

          if (IsNgayKy)
          {

              model.IsShowNgayKy = true;
          }
          else
          {

              model.IsShowNgayKy = model.IsShowNgayKy;
          }
          return true;
      })
        .WithMessage("Nếu ràng buộc ngày ký chọn, thì hiển thị ngày ký cũng phải chọn.")
        .OverridePropertyName(x => x.IsShowNgayKy);




        //RuleFor(model => model.IsNgayKy)
        //.NotEmpty()
        //.When(model => model.IsNgayKy)
        //.WithMessage("Ngày ký không được để trống.")
        //.OverridePropertyName(x => x.IsNgayKy);
        RuleFor(model => model.IsNoiDung)

      .NotEmpty()
      .When(model => model.IsNoiDung)
      .WithMessage("Nội dung không được để trống.")
      .OverridePropertyName(x => x.IsNoiDung)
      .Must((model, IsNoiDung) =>
      {

          if (IsNoiDung)
          {

              model.IsShowNoiDung = true;
          }
          else
          {

              model.IsShowNoiDung = model.IsShowNoiDung;
          }
          return true;
      })
          .WithMessage("Nếu ràng buộc nội dung chọn, thì hiển thị nội dung cũng phải chọn.")
       .OverridePropertyName(x => x.IsShowNoiDung);


        //RuleFor(model => model.IsNoiDung)
        //.NotEmpty()
        //.When(model => model.IsNoiDung)
        //.WithMessage("Nội dung không được để trống.")
        //.OverridePropertyName(x => x.IsNoiDung);
        RuleFor(model => model.IsTepTin)

   .NotEmpty()
   .When(model => model.IsTepTin)
   .WithMessage("Tệp tin không được để trống.")
   .OverridePropertyName(x => x.IsTepTin)
   .Must((model, IsTepTin) =>
   {

       if (IsTepTin)
       {

           model.IsShowTepTin = true;
       }
       else
       {

           model.IsShowTepTin = model.IsShowTepTin;
       }
       return true;
   })
   .WithMessage("Nếu ràng buộc tệp tin chọn, thì hiển thị tệp tin cũng phải chọn.")
   .OverridePropertyName(x => x.IsShowTepTin);



        //RuleFor(model => model.IsTepTin)
        //.NotEmpty()
        //.When(model => model.IsTepTin)
        //.WithMessage("Tệp tin không được để trống.")
        //.OverridePropertyName(x => x.IsTepTin);






    }
}
