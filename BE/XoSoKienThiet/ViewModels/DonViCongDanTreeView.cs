using XoSoKienThiet.Models.Core;

namespace XoSoKienThiet.ViewModels
{
    public class DonViCongDanTreeVM
    {
        public DonViCongDanTreeVM(string id, string label)
        {
            this.Id = id;
            this.Name = label;
        }
        public DonViCongDanTreeVM(DonViCongDanTreeVM model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
        }
        public DonViCongDanTreeVM(DonVi model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
          //  this.Selected = false;
          //  this.Opened = false;
        }
        
        public string Id { get; set; }
        public string Name { get; set; }
        
        
        public List<DonViCongDanTreeVM> Children { get; set; }
    }
    
    
    public class DonViCongDanTreeVMGetAll
    {
        public DonViCongDanTreeVMGetAll(string id, string label)
        {
            this.Id = id;
            this.Name = label;
        }
        public DonViCongDanTreeVMGetAll(DonViTreeVM model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
        }
        public DonViCongDanTreeVMGetAll(DonVi model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.Code = model.MaCoQuan; 
            this.CapDV = model.CapDV;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        
        public string Code { get; set; }
        
        public int CapDV { get; set; }
        
    }
}