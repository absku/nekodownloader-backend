using NekoDownloader.Core.Interfaces;

namespace NekoDownloader.Services.Schedules;

public class SyncComicSchedule(IUnitOfWork unitOfWork)
{
    public async Task SyncComics()
    {
        // Get all comics
        var comics = (await unitOfWork.ComicRepository.GetAsync(c => !c.Available)).ToList();
        
        // Sync each comic
        int i = 0;
        foreach (var comic in comics)
        {
            Console.WriteLine($"Syncing comic {i++}/{comics.Count}");
            // Sync comic
            var updateComic = await unitOfWork.GetSource(comic.Source).GetComic(comic.Link);
            comic.Chapters = updateComic.Chapters;
            comic.Available = true;
            await unitOfWork.ComicRepository.Update(comic);
            await unitOfWork.CommitAsync();
            i++;
        }
    }
}