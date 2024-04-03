using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NekoDownloader.Core.Entities;
using NekoDownloader.Core.Interfaces.Services;
using NekoDownloader.Web.Models;
using NekoDownloader.Web.Models.SaveModel;

namespace NekoDownloader.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ComicController(IComicService comicService, IMapper mapper) : ControllerBase
{
    [HttpGet("{uuid}")]
    public async Task<ActionResult<ComicModel>> GetComic(string uuid)
    {
        try
        {
            var comic = await comicService.GetByUuid(Guid.Parse(uuid));
            return Ok(mapper.Map<Comic, ComicModel>(comic));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{uuid}/chapters")]
    public async Task<ActionResult<IEnumerable<ChapterModel>>> GetChapters(string uuid)
    {
        try
        {
            var chapters = await comicService.GetChapters(Guid.Parse(uuid));
            return Ok(mapper.Map<IEnumerable<Chapter>, IEnumerable<ChapterModel>>(chapters));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{uuid}/chapters/{chapterUuid}")]
    public async Task<ActionResult<ChapterModel>> GetChapter(string uuid, string chapterUuid)
    {
        try
        {
            var chapter = await comicService.GetChapter(Guid.Parse(uuid), Guid.Parse(chapterUuid));
            return Ok(mapper.Map<Chapter, ChapterWithPagesModel>(chapter));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ComicModel>>> GetAll()
    {
        try
        {
            var comics = await comicService.GetAll();
            return Ok(mapper.Map<IEnumerable<Comic>, IEnumerable<ComicModel>>(comics));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{uuid}/download/cbz")]
    public async Task<ActionResult> DownloadCbz(string uuid)
    {
        try
        {
            var cbz = await comicService.DownloadCbz(Guid.Parse(uuid));
            var comic = await comicService.GetByUuid(Guid.Parse(uuid));
            return File(cbz, "application/zip", $"{comic.Title}.cbz");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<ComicModel>> CreateComic([FromBody] ComicSaveModel comicSaveModel)
    {
        try
        {
            var createdComic = await comicService.Create(mapper.Map<ComicSaveModel, Comic>(comicSaveModel));
            return Ok(mapper.Map<Comic, ComicModel>(createdComic));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpDelete("{uuid}")]
    public async Task<ActionResult> DeleteComic(string uuid)
    {
        try
        {
            await comicService.Delete(Guid.Parse(uuid));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("{uuid}")]
    public async Task<ActionResult<ComicModel>> UpdateComic(string uuid, [FromBody] ComicSaveModel comicSaveModel)
    {
        try
        {
            var updatedComic = await comicService.Update(Guid.Parse(uuid), mapper.Map<ComicSaveModel, Comic>(comicSaveModel));
            return Ok(mapper.Map<Comic, ComicModel>(updatedComic));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("{uuid}/sync")]
    public async Task<ActionResult> SyncComic(string uuid)
    {
        try
        {
            await comicService.SyncComic(Guid.Parse(uuid));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("{uuid}/chapters/{chapterUuid}/sync")]
    public async Task<ActionResult> SyncChapter(string uuid, string chapterUuid)
    {
        try
        {
            await comicService.SyncChapter(Guid.Parse(uuid), Guid.Parse(chapterUuid));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}