using Microsoft.AspNetCore.Mvc;
using NekoDownloader.Core.Interfaces.Services;

namespace NekoDownloader.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PageController(IPageService pageService) : ControllerBase
{
    [HttpGet("{uuid}")]
    public async Task<FileContentResult> GetPage(string uuid)
    {
        var page = await pageService.GetByUuid(Guid.Parse(uuid));
        return File(page, "image/jpeg");
    }
}