using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using XoSoKienThiet.Models.Core;
namespace XoSoKienThiet.Models.NghiepVu;

public class Slider: Audit, TEntity<string>
{
    [BsonIgnore]
    public List<FileShort> Files {
        get;
        set;
    }
    
    public FileShort File {
        get;
        set;
    }
    public int Sort { set; get; }
}
