using System.Collections.ObjectModel;

namespace NekoDownloader.Core.Entities;

public class Chapter
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public float Number { get; set; }
    public string Title { get; set; } = null!;
    public bool Available { get; set; } = false;
    public string Link { get; set; } = null!;
    public Guid ComicUuid { get; set; }
    public Comic Comic { get; set; } = null!;
    public ICollection<Page> Pages { get; set; } = new Collection<Page>();
}