using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using XoSoKienThiet.Constants;

namespace XoSoKienThiet.Models.Core
{
   
    public class Menu : Audit, TEntity<string>
    {
        public string Path { get; set; }
        public string Icon { get; set; }
       // public bool IsShow { get; set; } = true;

        /*
        private string _resource;

        public string Resource
        {
            get
            {
                return  FormatterString.ConvertToUnsign(Name).Replace(" ", "");
            }
            set
            {
            }
        }*/
        public string ParentId { get; set; }
        public int Level { get; set; } = 0;
        public int Sort { get; set; }

        public bool isPrivate { get; set; } = false;
        
        public bool IsTieuDe { get; set; }
        public bool IsMoTa { get; set; }
        public bool IsTrichYeu { get; set; } 
        public bool IsKyHieu { get; set; } 
        public bool IsNgayXuatBan { get; set; }
        public bool IsAnhDaiDien { get; set; } 
        public bool IsNgayKy { get; set; } 
        public bool IsNoiDung { get; set; } 
        public bool IsTepTin { get; set; }

        public bool IsShowTieuDe { get; set; }
        public bool IsShowMoTa { get; set; }
        public bool IsShowTrichYeu { get; set; }
        public bool IsShowKyHieu { get; set; }
        public bool IsShowNgayXuatBan { get; set; }
        public bool IsShowAnhDaiDien { get; set; }
        public bool IsShowNgayKy { get; set; }
        public bool IsShowNoiDung { get; set; }
        public bool IsShowTepTin { get; set; }

        public bool IsTrangChu { get; set; } = false;
        public bool IsMenu { get; set; } = false;
        public bool IsDanhMuc { get; set; } = false;



        public List<string> ListAction
        {
            get;
            set;
        } = new List<string>();



        public void SetListAction(string Name)
        {
            this.ListAction = new List<string>();
            foreach (var item in ListActionDefault.listAction)
            {
                var actionName = item + "-" + FormatterString.ConvertToUnsign(Name).Replace(" ", "") + "-" + Id;
                this.ListAction.Add(actionName);
            }
        }
        
        
        [BsonIgnore]
        public bool IsChecked { get; set; } = false;
        
        
    }

    
    
    
    
    
    

    public class MenuShort
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public string ParentId { get; set; }
        public int Sort { get; set; }

        public MenuShort(Menu menu)
        {
            this.Id = menu.Id;
            this.Name = menu.Name;
            this.Path = menu.Path;
            this.ParentId = menu.ParentId;
            this.Icon = menu.Icon;
            this.Sort = menu.Sort;
        }

    }
    public class MenuList
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        public string ListAction { get; set; }
    }
    public class MenuListShort
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Resource
        {
            get
            {
                return FormatterString.ConvertToUnsign(Name).Replace(" ", "");
            }
            set
            {
            }
        }
        public string ListAction { get; set; }
    }
   
    public class MenuCongDan1
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
      
        public string Id { get; set; }
        public string Name { get; set; }
       





    }
}