using System.Collections.ObjectModel;

namespace NekoDownloader.Web.Models;

public class ChapterModel
{
    public Guid Uuid { get; set; }
    public float Number { get; set; }
    public string Title { get; set; } = null!;
    public bool Available { get; set; }
    public string Link { get; set; } = null!;
}

public class ChapterWithPagesModel : ChapterModel
{
    public ICollection<PageModel> Pages { get; set; } = new Collection<PageModel>();
}