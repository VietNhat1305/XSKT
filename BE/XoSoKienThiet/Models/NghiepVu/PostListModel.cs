namespace XoSoKienThiet.Models.NghiepVu;

public class PostListModel
{
    public string Id { get; set; }
    public string Name { get; set; }

    public List<ModelContentShort> DanhSach { get; set; } = new List<ModelContentShort>();
}