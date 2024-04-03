namespace NekoDownloader.Core.Interfaces.Services;

public interface IPageService
{
   Task<byte[]> GetByUuid(Guid uuid);
}