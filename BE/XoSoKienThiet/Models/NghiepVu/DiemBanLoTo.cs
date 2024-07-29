using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using XoSoKienThiet.Models.Core;
namespace XoSoKienThiet.Models.NghiepVu
{
    public class DiemBanLoTo : Audit, TEntity<string>
    {
        public FileShort File { get; set; }
    }
}
