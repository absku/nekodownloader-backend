namespace NekoDownloader.Web.Models;

public class PageModel
{
    public Guid Uuid { get; set; }
    public string Link { get; set; } = null!;
    public bool Available { get; set; }
    public int Number { get; set; }
}