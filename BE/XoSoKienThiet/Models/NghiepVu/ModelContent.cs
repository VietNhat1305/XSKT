using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Models.Core;
namespace XoSoKienThiet.Models.NghiepVu;

public class ModelContent: Audit, TEntity<string>
{
    public string NoiDung { get; set; }

    public string  HoVaTen {
        get;
        set;
    }
    
    public string SoDienThoai { get; set; }
    public string Email { get; set; }
    public string DiaChi { get; set; }
    public List<FileShort> Files { get; set; }

    public List<MenuCongDanShort>Menu {  get; set; }
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
        get
        {
            if (NgayXuatBan.HasValue)
            {
                // Kiểm tra xem NgayXuatBan có phải là UTC không và chuyển đổi nếu cần
                DateTime localTime = NgayXuatBan.Value.Kind == DateTimeKind.Utc
                    ? NgayXuatBan.Value.ToLocalTime()
                    : NgayXuatBan.Value;

                // Định dạng chuỗi để bao gồm cả ngày và giờ
                string formattedDate = localTime.ToString($"{FormatTime.FORMAT_DATE_CORE} HH:mm:ss");

                return formattedDate;
            }
            else
            {
                return "";
            }
        }
    }
    public string TrichYeu { get; set; }
    public string MoTa { get; set; }
}




public class ModelContentShort 
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    [JsonProperty(PropertyName = "_id")]
    public string Id { get; set; }
    public string Name { get; set; }
    public string MoTa { get; set; }
    public FileShort FileImage { get; set; }

}


public class ContentPagingParam
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
    
    public string? ContentId { get; set;}
    
}