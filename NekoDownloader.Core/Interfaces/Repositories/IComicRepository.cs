using NekoDownloader.Core.Entities;

namespace NekoDownloader.Core.Interfaces.Repositories;

public interface IComicRepository : IBaseRepository<Comic>
{
    Task<Comic> GetByUuidFullAsync(Guid uuid);
}