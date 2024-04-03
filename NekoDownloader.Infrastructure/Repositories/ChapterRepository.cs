using Microsoft.EntityFrameworkCore;
using NekoDownloader.Core.Entities;
using NekoDownloader.Core.Interfaces.Repositories;
using NekoDownloader.Infrastructure.Data;

namespace NekoDownloader.Infrastructure.Repositories;

public class ChapterRepository : BaseRepository<Chapter>,  IChapterRepository
{
    public ChapterRepository(AppDbContext context) : base(context)
    {
    }
    
    public async Task<Chapter> GetByUuidFullAsync(Guid uuid)
    {
        return await Context.Chapters
            .Include(c => c.Comic)
            .Include(c => c.Pages)
            .FirstOrDefaultAsync(c => c.Uuid == uuid) ?? throw new InvalidOperationException();
    }
}