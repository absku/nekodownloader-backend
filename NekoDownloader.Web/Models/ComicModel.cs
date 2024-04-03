using System.Collections.ObjectModel;
using NekoDownloader.Core.Enums;

namespace NekoDownloader.Web.Models;

public class ComicModel
{
    public Guid Uuid { get; set; }
    public string Title { get; set; } = null!;
    public string Resume { get; set; } = null!;
    public string Link { get; set; } = null!;
    public ComicType Type { get; set; }
    public ICollection<GenreType> Genres { get; set; } = new Collection<GenreType>();
    public StateType State { get; set; }
    public ICollection<ChapterModel> Chapters { get; set; } = new Collection<ChapterModel>();
}