using NekoDownloader.Core.Entities;

namespace NekoDownloader.Core.Interfaces.Sources;

public interface IBaseSource
{
    Task<Comic> GetComic(string link);
    Task<IEnumerable<Page>> GetChapter(string link);
    Task<byte[]> GetPage(string link);
}