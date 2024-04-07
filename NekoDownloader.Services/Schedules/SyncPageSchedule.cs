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
        var i = 0;
        foreach (var page in pages.OrderBy(p => p.ChapterUuid))
        {
            Console.WriteLine($"Syncing page {i++}/{pages.Count}");
            var chapter = await unitOfWork.ChapterRepository.GetByUuidAsync(page.ChapterUuid);
            var comic = await unitOfWork.ComicRepository.GetByUuidAsync(chapter.ComicUuid);
            // Sync page
            try
            {
                var updatePage = await unitOfWork.GetSource(comic.Source).GetPage(page.Link);
                page.Image = Path.Combine(comic.Title, chapter.Number.ToString(CultureInfo.InvariantCulture),
                    page.Number + ".jpg");
                var pagePath = Path.Combine(savePath, page.Image);
                CreateDirectoryIfNotExists(pagePath);
                await File.WriteAllBytesAsync(pagePath, updatePage);
                page.Available = true;
                await unitOfWork.PageRepository.Update(page);
                await unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            i++;
        }
    }

    private static void CreateDirectoryIfNotExists(string path)
    {
        var directory = Path.GetDirectoryName(path)!;
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}