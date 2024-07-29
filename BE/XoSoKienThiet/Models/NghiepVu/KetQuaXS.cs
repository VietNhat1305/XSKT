using MongoDB.Bson.Serialization.Attributes;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Models.Core;
namespace XoSoKienThiet.Models.NghiepVu;

public class KetQuaXS: Audit, TEntity<string>
{
    public string KyVe {  get; set; }
    public DateTime? Date { get; set; }

    [BsonIgnore]
    public string CreatedAtShow
    {
        get { return CreatedAt.HasValue ? CreatedAt.Value.ToLocalTime().ToString(FormatTime.FORMAT_DATE) : ""; }
    }

    public int? DateShow
    {
        get { return Date.HasValue ? FormatDate.ConvertDatetimeQG(Date) : null; }
        set { }
    }
    public string Giai8 { get; set; }
    public string Giai7 { get; set; }
    public List<string> Giai6 { get; set; }
    public string Giai5 { get; set; }
    public List<string> Giai4 { get; set; }
    public List<string> Giai3 { get; set; }
    public string Giai2 { get; set; }
    public string Giai1 { get; set; }
    public string GiaiDB { get; set; }
    public FileShort File { get; set; }
}
public class KetQuaXSShort
{
    public string Giai { get; set; }
    public List<string> KetQua {  get; set; }

}

public class KhoanCanh
{
    public float Start { get; set; }

    public float Step { get; set; }

    public float TextWidth { get; set; }

}