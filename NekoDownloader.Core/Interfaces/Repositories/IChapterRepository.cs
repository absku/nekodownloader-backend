using NekoDownloader.Core.Entities;

namespace NekoDownloader.Core.Interfaces.Repositories;

public interface IChapterRepository: IBaseRepository<Chapter>
{
    Task<Chapter> GetByUuidFullAsync(Guid uuid);
}