using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using XoSoKienThiet.Models.Core;
namespace XoSoKienThiet.Models.NghiepVu;

public class LienKet : Audit, TEntity<string>
{
    public string Link { get; set; }
}
    
