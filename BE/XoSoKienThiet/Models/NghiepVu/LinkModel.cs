using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using XoSoKienThiet.Models.Core;
namespace XoSoKienThiet.Models.NghiepVu;

public class LinkModel : Audit, TEntity<string>
{
    public FileShort File
    {
        get;
        set;
    }
    public string Link { get; set; }
    public string MoTa    { get; set; }
}
    
