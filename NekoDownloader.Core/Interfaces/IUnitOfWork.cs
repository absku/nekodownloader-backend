using NekoDownloader.Core.Enums;
using NekoDownloader.Core.Interfaces.Repositories;
using NekoDownloader.Core.Interfaces.Sources;

namespace NekoDownloader.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IComicRepository ComicRepository { get; }
    IChapterRepository ChapterRepository { get; }
    IPageRepository PageRepository { get; }
    
    IBaseSource IkigaiMangasSource { get; }
    IBaseSource LectorKnsSource { get; }
    IBaseSource DragonTranslationSource { get; }
    
    IBaseSource GetSource(SourceType sourceType);
    
    Task<int> CommitAsync();
}