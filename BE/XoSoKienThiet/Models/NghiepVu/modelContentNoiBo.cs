using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Models.Core;

namespace XoSoKienThiet.Models.NghiepVu;

public class modelContentNoiBo : Audit, TEntity<string>
{
    public string NoiDung { get; set; }
    
    public List<FileShort> Files { get; set; }

    public List<MenuCongDan1>Menu {  get; set; }
    public DateTime? NgayKy { get;set; }
    public string NgayKyShow
    {
        get { return NgayKy.HasValue ? NgayKy.Value.ToLocalTime().ToString(FormatTime.FORMAT_DATE_CORE) : ""; }
    }
    public string SoKiHieu { get; set; }
    public FileShort FileImage { get; set; }
    
    public bool IsPublic { get; set; } = true;
    public DateTime? NgayXuatBan { get; set; }
    public string NgayXuatBanShow
    {
        get { return NgayXuatBan.HasValue ? NgayXuatBan.Value.ToLocalTime().ToString(FormatTime.FORMAT_DATE_CORE) : ""; }
    }
    public string TrichYeu { get; set; }
    public string MoTa { get; set; }
}

public class ModelContentNoiBoShort 
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    [JsonProperty(PropertyName = "_id")]
    public string Id { get; set; }
    public string Name { get; set; }
    public string MoTa { get; set; }
    public FileShort FileImage { get; set; }

}


public class ContentNoiBoPagingParam
{
    public int Start { get; set; } = 1;
    public int Limit { get; set; } = 10;
    
    public int Skip
    {
        get
        {
            return (Start > 0 ? Start - 1 : 0) * Limit;
        }
    }

    public string? SortBy { get; set; }

    public bool SortDesc { get; set; }
    public string? Content { get; set; }
    public string? MenuId { get; set; }

    
}