using XoSoKienThiet.Models.Core;

namespace XoSoKienThiet.Models.NghiepVu
{
    public class LienHeModel:Audit, TEntity<string>
    {
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string VanPhongDaiDien { get; set; }
        public int MaSoThue { get; set; }
        public string NguoiBienTap { get; set; }
        
    }
}
