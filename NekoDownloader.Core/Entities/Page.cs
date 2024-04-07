namespace NekoDownloader.Core.Entities;

public class Page
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public string Link { get; set; } = null!;
    public bool Available { get; set; } = false;
    public string Image { get; set; } = null!;
    public int Number { get; set; }
    public Guid ChapterUuid { get; set; }
    public Chapter Chapter { get; set; } = null!;
}