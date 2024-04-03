using NekoDownloader.Core.Entities;

namespace NekoDownloader.Core.Interfaces.Services;

public interface IComicService : IBaseService<Comic>
{
    Task<IEnumerable<Chapter>> GetChapters(Guid comicId);
    Task<Chapter> GetChapter(Guid comicId, Guid chapterId);
    Task SyncComic(Guid comicId);
    Task SyncChapter(Guid comicId, Guid chapterId);
    Task<Byte[]> DownloadCbz(Guid comicId);
}