using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using XoSoKienThiet.Models.Core;
namespace XoSoKienThiet.Models.NghiepVu;

public class ThuVienAnh : Audit, TEntity<string>
{
    public string LoaiAnhId { get; set; }
    public FileShort File { get;  set; }

}
    
