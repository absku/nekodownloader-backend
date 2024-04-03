using NekoDownloader.Core.Interfaces;
using NekoDownloader.Core.Interfaces.Services;

namespace NekoDownloader.Services;

public class PageService(IUnitOfWork unitOfWork) : IPageService
{
    public async Task<byte[]> GetByUuid(Guid uuid)
    {
        var page = await unitOfWork.PageRepository.GetByUuidAsync(uuid);
        return await File.ReadAllBytesAsync(page.Image);
    }
}