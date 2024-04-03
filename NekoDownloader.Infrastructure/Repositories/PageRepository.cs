using NekoDownloader.Core.Entities;
using NekoDownloader.Core.Interfaces.Repositories;
using NekoDownloader.Infrastructure.Data;

namespace NekoDownloader.Infrastructure.Repositories;

public class PageRepository : BaseRepository<Page>,  IPageRepository
{
    public PageRepository(AppDbContext context) : base(context)
    {
    }
}