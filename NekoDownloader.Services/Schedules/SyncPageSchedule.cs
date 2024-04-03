using System.Globalization;
using Microsoft.Extensions.Options;
using NekoDownloader.Core.Configs;
using NekoDownloader.Core.Interfaces;

namespace NekoDownloader.Services.Schedules;

public class SyncPageSchedule(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
{
    public async Task SyncPages()
    {
        // Get all pages
        var pages = (await unitOfWork.PageRepository.GetAsync(p => !p.Available)).ToList();
        var savePath = appSettings.Value.DownloadPath;
        
        // Sync each page
        int i = 0;
        foreach (var page in pages.OrderBy(p => p.ChapterUuid))
        {
            Console.WriteLine($"Syncing page {i++}/{pages.Count}");
            var chapter = await unitOfWork.ChapterRepository.GetByUuidAsync(page.ChapterUuid);
            var comic = await unitOfWork.ComicRepository.GetByUuidAsync(chapter.ComicUuid);
            // Sync page
            var updatePage = await unitOfWork.GetSource(comic.Source).GetPage(page.SourceLink);
            var pagePath = Path.Combine(savePath, comic.Title, chapter.Number.ToString(CultureInfo.InvariantCulture), page.Number + ".jpg");
            CreateDirectoryIfNotExists(pagePath);
            await File.WriteAllBytesAsync(pagePath, updatePage);
            page.Image = pagePath;
            page.Link = $"{appSettings.Value.Url}/page/{page.Uuid}";
            page.Available = true;
            await unitOfWork.PageRepository.Update(page);
            await unitOfWork.CommitAsync();
            i++;
        }
    }
    
    private void CreateDirectoryIfNotExists(string path)
    {
        var directory = Path.GetDirectoryName(path)!;
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}