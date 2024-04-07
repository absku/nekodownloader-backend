using Microsoft.Extensions.Options;
using NekoDownloader.Core.Configs;
using NekoDownloader.Core.Interfaces;
using NekoDownloader.Core.Interfaces.Services;

namespace NekoDownloader.Services;

public class PageService(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings) : IPageService
{
    public async Task<byte[]> GetByUuid(Guid uuid)
    {
        var page = await unitOfWork.PageRepository.GetByUuidAsync(uuid);
        page.Image = Path.Combine(appSettings.Value.DownloadPath, page.Image);
        return await File.ReadAllBytesAsync(page.Image);
    }
}