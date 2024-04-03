using System.Globalization;
using System.IO.Compression;
using NekoDownloader.Core.Entities;
using NekoDownloader.Core.Interfaces;
using NekoDownloader.Core.Interfaces.Services;

namespace NekoDownloader.Services;

public class ComicService(IUnitOfWork unitOfWork) : IComicService
{
    public async Task<Comic> GetByUuid(Guid uuid)
    {
        return await unitOfWork.ComicRepository.GetByUuidAsync(uuid);
    }

    public async Task<IEnumerable<Comic>> GetAll()
    {
        return await unitOfWork.ComicRepository.GetAllAsync();
    }
    
    public async Task<IEnumerable<Chapter>> GetChapters(Guid comicId)
    {
        return await unitOfWork.ChapterRepository.GetAsync(c => c.ComicUuid == comicId);
    }

    public async Task<Chapter> GetChapter(Guid comicId, Guid chapterId)
    {
        return await unitOfWork.ChapterRepository.GetByUuidFullAsync(chapterId);
    }

    public async Task SyncComic(Guid comicId)
    {
        var actualComic = await unitOfWork.ComicRepository.GetByUuidFullAsync(comicId);
        var newComic = await unitOfWork.GetSource(actualComic.Source).GetComic(actualComic.Link);
        if (newComic.Chapters.Count > actualComic.Chapters.Count)
        {
            var newChapters = newComic.Chapters.Where(c => actualComic.Chapters.All(ac => ac.Link != c.Link));
            foreach (var chapter in newChapters)
            {
                actualComic.Chapters.Add(chapter);
            }
            await unitOfWork.CommitAsync();
        }
    }

    public async Task SyncChapter(Guid comicId, Guid chapterId)
    {
        var actualChapter = await unitOfWork.ChapterRepository.GetByUuidFullAsync(chapterId);
        var newChapterPages = await unitOfWork.GetSource(actualChapter.Comic.Source).GetChapter(actualChapter.Link);
        actualChapter.Pages = newChapterPages.ToList();
        actualChapter.Available = false;
        await unitOfWork.CommitAsync();
    }

    public async Task<byte[]> DownloadCbz(Guid comicId)
    {
        var comic = await unitOfWork.ComicRepository.GetByUuidFullAsync(comicId);
        var chapters = new List<Chapter>();
        foreach (var chapter in comic.Chapters)
        {
            var c = await unitOfWork.ChapterRepository.GetByUuidFullAsync(chapter.Uuid);
            chapters.Add(c);
        }

        try
        {
            using var memoryStream = new MemoryStream();
            using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);
            foreach (var chapter in chapters)
            {
                foreach (var page in chapter.Pages)
                {
                    var path = Path.Combine(chapter.Number.ToString(CultureInfo.InvariantCulture), $"{page.Number}.jpg");
                    var pageEntry = archive.CreateEntry(path, CompressionLevel.Fastest);
                    await using var pageStream = pageEntry.Open();
                    var pageBytes = await File.ReadAllBytesAsync(page.Image);
                    await pageStream.WriteAsync(pageBytes.AsMemory(0, (int) new FileInfo(page.Image).Length));
                }
            }
            return memoryStream.ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Error creating cbz");
        }
    }

    public async Task<Comic> Create(Comic newEntity)
    {
        await unitOfWork.ComicRepository.AddAsync(newEntity);
        await unitOfWork.CommitAsync();
        return newEntity;
    }

    public Task<Comic> Update(Guid entityToBeUpdatedId, Comic newEntityValues)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(Guid uuid)
    {
        var comic = await unitOfWork.ComicRepository.GetByUuidAsync(uuid);
        unitOfWork.ComicRepository.Remove(comic);
        await unitOfWork.CommitAsync();
    }
}
