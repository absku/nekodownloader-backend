using NekoDownloader.Core.Enums;
using NekoDownloader.Core.Interfaces;
using NekoDownloader.Core.Interfaces.Repositories;
using NekoDownloader.Core.Interfaces.Sources;
using NekoDownloader.Infrastructure.Repositories;
using NekoDownloader.Infrastructure.Sources;

namespace NekoDownloader.Infrastructure.Data;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private ComicRepository _comicRepository = null!;
    private ChapterRepository _chapterRepository = null!;
    private PageRepository _pageRepository = null!;
    
    private IBaseSource _ikigaiMangasSource = null!;
    private IBaseSource _lectorKnsSource = null!;

    public IComicRepository ComicRepository => _comicRepository ??= new ComicRepository(context);
    
    public IChapterRepository ChapterRepository => _chapterRepository ??= new ChapterRepository(context);
    
    public IPageRepository PageRepository => _pageRepository ??= new PageRepository(context);
    
    public IBaseSource IkigaiMangasSource => _ikigaiMangasSource ??= new IkigaiMangasSource();
    
    public IBaseSource LectorKnsSource => _lectorKnsSource ??= new LectorknsSource();
    
    public IBaseSource DragonTranslationSource => new DragonTranslationSource();

    public IBaseSource GetSource(SourceType sourceType)
    {
        return sourceType switch
        {
            SourceType.ikigaimangas => IkigaiMangasSource,
            SourceType.lectorkns => LectorKnsSource,
            SourceType.dragontranslation => DragonTranslationSource,
            _ => throw new ArgumentOutOfRangeException(nameof(sourceType), sourceType, null)
        };
    }
    
    public async Task<int> CommitAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}