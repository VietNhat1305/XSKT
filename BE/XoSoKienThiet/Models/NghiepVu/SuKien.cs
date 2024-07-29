using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using XoSoKienThiet.Models.Core;
namespace XoSoKienThiet.Models.NghiepVu;

public class SuKien: Audit, TEntity<string>
{
    public string Content {get; set;}
    
  
}
