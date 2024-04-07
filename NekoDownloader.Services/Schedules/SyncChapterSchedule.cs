using NekoDownloader.Core.Interfaces;

namespace NekoDownloader.Services.Schedules;

public class SyncChapterSchedule(IUnitOfWork unitOfWork)
{
    public async Task SyncChapters()
    {
        // Get all comics
        var chapters = (await unitOfWork.ChapterRepository.GetAsync(c => !c.Available)).ToList();

        // Sync each comic
        int i = 0;
        foreach (var chapter in chapters.OrderBy(c => c.ComicUuid))
        {
            Console.WriteLine($"Syncing chapter {i++}/{chapters.Count}");
            var comic = await unitOfWork.ComicRepository.GetByUuidAsync(chapter.ComicUuid);
            var source = unitOfWork.GetSource(comic.Source);
            var chapterPages = await source.GetChapter(chapter.Link);
            chapter.Pages = chapterPages.ToList();
            chapter.Available = true;
            await unitOfWork.ChapterRepository.Update(chapter);
            await unitOfWork.CommitAsync();
            i++;
        }
    }
}