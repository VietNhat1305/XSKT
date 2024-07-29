using XoSoKienThiet.Models.Core;
namespace XoSoKienThiet.Models.NghiepVu;

public class Home: Audit, TEntity<string>
{
    public List<FileModel> BienBanKQ { get; set; }

    public List<FileModel> KQXS { get; set; }

    public string Content { get; set; }

    public List<FileModel> DiemMuaXoSo { get; set; }

    public MenuCongDan Menu { get; set; }

}
