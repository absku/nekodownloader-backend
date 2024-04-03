using NekoDownloader.Core.Enums;

namespace NekoDownloader.Web.Models.SaveModel;

public class ComicSaveModel
{
    public string Title { get; set; } = null!;
    public string Link { get; set; } = null!;
    public SourceType Source { get; set; }
}