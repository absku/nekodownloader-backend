using System.Collections.ObjectModel;
using NekoDownloader.Core.Enums;

namespace NekoDownloader.Core.Entities;

public class Comic
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = null!;
    public string Resume { get; set; } = null!;
    public string Link { get; set; } = null!;
    public ComicType Type { get; set; }
    public ICollection<GenreType> Genres { get; set; } = new Collection<GenreType>();
    public StateType State { get; set; }
    public SourceType Source { get; set; }
    public bool Available { get; set; } = false;
    public ICollection<Chapter> Chapters { get; set; } = new Collection<Chapter>();
}