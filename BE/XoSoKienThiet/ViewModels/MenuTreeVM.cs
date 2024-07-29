using Newtonsoft.Json;
using XoSoKienThiet.Models.Core;

namespace XoSoKienThiet.ViewModels
{
    public class MenuTreeVM
    {
     
        public MenuTreeVM(Menu model)
        {
            this.Id = model.Id;
            this.Label = model.Name;
            this.Link = model.Path ?? "";
            this.Icon = model.Icon ?? "";
            this.ParentId = model.ParentId;
            this.Level = model.Level;
            
            this.IsTieuDe = model.IsTieuDe;
            this.IsMoTa = model.IsMoTa;
            this.IsTrichYeu = model.IsTrichYeu;
            this.IsKyHieu = model.IsKyHieu;
            this.IsNgayXuatBan = model.IsNgayXuatBan;
            this.IsAnhDaiDien = model.IsAnhDaiDien;
            this.IsNgayKy = model.IsNgayKy;
            this.IsNoiDung = model.IsNoiDung;
            this.IsTepTin = model.IsTepTin;

            this.IsShowTieuDe = model.IsShowTieuDe;
            this.IsShowMoTa = model.IsShowMoTa;
            this.IsShowTrichYeu = model.IsShowTrichYeu;
            this.IsShowKyHieu = model.IsShowKyHieu;
            this.IsShowNgayXuatBan = model.IsShowNgayXuatBan;
            this.IsShowAnhDaiDien = model.IsShowAnhDaiDien;
            this.IsShowNgayKy = model.IsShowNgayKy;
            this.IsShowNoiDung = model.IsShowNoiDung;
            this.IsShowTepTin = model.IsShowTepTin;


            this.IsTrangChu = false;
            this.IsMenu = false;
            this.IsDanhMuc = false;
        }
        
        public string Id { get; set; }
        public string Label { get; set; }
        public List<MenuTreeVM> Children { get; set; }
        public bool Opened { get; set; } = false;
        public string ParentId { get; set; }= "";
        public string Link { get; set; } = "";
        public string Icon { get; set; } = "";
        public int  Level { get; set; } 
        public bool Selected { get; set; } = false;
        
        public bool IsTieuDe { get; set; }
        public bool IsMoTa { get; set; }
        public bool IsTrichYeu { get; set; }
        public bool IsKyHieu { get; set; }
        public bool IsNgayXuatBan { get; set; }
        public bool IsAnhDaiDien { get; set; }
        public bool IsNgayKy { get; set; }
        public bool IsNoiDung { get; set; }
        public bool IsTepTin { get; set; }
        //
        public bool IsShowTieuDe { get; set; }
        public bool IsShowMoTa { get; set; }
        public bool IsShowTrichYeu { get; set; }
        public bool IsShowKyHieu { get; set; }
        public bool IsShowNgayXuatBan { get; set; }
        public bool IsShowAnhDaiDien { get; set; }
        public bool IsShowNgayKy { get; set; }
        public bool IsShowNoiDung { get; set; }
        public bool IsShowTepTin { get; set; }

        public bool IsTrangChu { get; set; } = false;
        public bool IsMenu { get; set; } = false;
        public bool IsDanhMuc { get; set; } = false;
    }
    public class MenuTreeVMGetAll
    {
        public MenuTreeVMGetAll(Menu model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.CapDV = model.Level;
            this.Link = model.Path;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        
        public string Link { get; set; }
        
        public int CapDV { get; set; }
        
    }
    
    
}