using Microsoft.EntityFrameworkCore;
using NekoDownloader.Core.Entities;
using NekoDownloader.Core.Interfaces.Repositories;
using NekoDownloader.Infrastructure.Data;

namespace NekoDownloader.Infrastructure.Repositories;

public class ComicRepository : BaseRepository<Comic>,  IComicRepository
{
    public ComicRepository(AppDbContext context) : base(context)
    {
    }
    
    public async Task<Comic> GetByUuidFullAsync(Guid uuid)
    {
        return await Context.Comics
            .Include(c => c.Chapters)
            .FirstOrDefaultAsync(c => c.Uuid == uuid) ?? throw new InvalidOperationException();
    }
}