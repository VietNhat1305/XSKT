using XoSoKienThiet.Models.Core;
namespace XoSoKienThiet.Models.NghiepVu;

public class LoaiAnh : Audit, TEntity<string>
{
    public FileShort File {  get; set; }
    public int Count { get; set; }
}
    
