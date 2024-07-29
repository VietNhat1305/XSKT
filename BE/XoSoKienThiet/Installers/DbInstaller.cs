
using QuanLyKhuCongNghiep.Services.NghiepVu;
using XoSoKienThiet.Interfaces.Auth;
using XoSoKienThiet.Interfaces.Common;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Interfaces.NghiepVu;
using XoSoKienThiet.Services.Auth;
using XoSoKienThiet.Services.Common;
using XoSoKienThiet.Services.Core;
using XoSoKienThiet.Services.NghiepVu;

namespace XoSoKienThiet.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<DataContext>();

            #region Auth 
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IIdentityService, IdentityService>();
            #endregion

            #region Common

            services.AddScoped<ICommonService, CommonService>();

            #endregion
            
            #region Core 

            services.AddScoped<IUnitRoleService, UnitRoleService>();
            


            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IMenuCongDanService, MenuCongDanService>();
            services.AddScoped<IDanhMucService, DanhMucService>();

         

            services.AddScoped<IUserService, UserService>();



           
       
           
           
            
            services.AddScoped<IFileMinioService, FileMinioService>();

            #endregion


            #region Nghiệp vụ  
            services.AddScoped<ILienHeService, LienHeService>();
            services.AddScoped<ILoggingService,LoggingService>();
            services.AddScoped<IModelContentService, ModelContentService>();
            services.AddScoped<IHeaderService, HeaderService>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<ISuKienService, SuKienService>();
            services.AddScoped<ILinkService, LinkService>();
            services.AddScoped<IEditImgService, EditImgService>();
            services.AddScoped<IThuVienAnhService, ThuVienAnhService>();
            services.AddScoped<IDiemLoToService, DiemLoToService>();
            services.AddScoped<IDiemBanLoToService, DiemBanLoToService>();
            services.AddScoped<ILienKietService, LienKietService>();
            services.AddScoped<ILinkYtbService, LinkYtbService>();
            services.AddScoped<IImageHomeService, ImageHomeService>();
            services.AddScoped<IBienBanService, BienBanService>();
            services.AddScoped<IModelContentNoiBoService, ModelContentNoiBoService>();
            services.AddScoped<ILoaiAnhService, LoaiAnhService>();



            #endregion


        }
    }
}
