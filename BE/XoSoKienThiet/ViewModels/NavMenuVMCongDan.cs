using Newtonsoft.Json;
using XoSoKienThiet.Models.Core;

namespace XoSoKienThiet.ViewModels
{
    public class NavMenuVMCongDan
    {
        public NavMenuVMCongDan(MenuCongDan model)
        {
            this.Id = model.Id  ?? "";
            this.Name = model.Name  ?? "";
            this.Icon = model.Icon  ?? "";
            this.ParentId = model.ParentId ?? "";
            this.Link = string.IsNullOrEmpty(model.Path)? "/" : model.Path;
            this.IsTitle = model.Level == 0 ? true : false;
        }
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string ParentId { get; set; }
        public string Link { get; set; }
        public bool IsTitle { get; set; }
        public int Level { get; set; }
     //   public List<NavMenuVM> SubItems { get; set; } = new List<NavMenuVM>();

        public List<NavMenuVMCongDan> Children { get; set; } = new List<NavMenuVMCongDan>();
    }
}