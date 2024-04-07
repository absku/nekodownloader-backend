using System.Globalization;
using AngleSharp.Html.Parser;
using NekoDownloader.Core.Entities;
using NekoDownloader.Core.Interfaces.Sources;
using RestSharp;

namespace NekoDownloader.Infrastructure.Sources;

public class DragonTranslationSource : IBaseSource
{
    private const string ChaptersUrl = "https://dragontranslation.net/manga/{serie}";
    
    public async Task<Comic> GetComic(string serie)
    {
        var client = new RestClient(ChaptersUrl.Replace("{serie}", serie));
        var request = new RestRequest();
        var response = await client.ExecuteAsync(request);
        if (!response.IsSuccessful || response.Content == null)
            throw new Exception("Error getting comic");

        var parser = new HtmlParser();
        var html = parser.ParseDocument(response.Content);
        
        var chapters = html.QuerySelectorAll(".chapter-link > a");
        var comic = new Comic
        {
            Chapters = chapters.Select(x => new Chapter
            {
                Title = x.QuerySelector("p")!.TextContent,
                Number = float.Parse(x.QuerySelector("p")!.TextContent.Split(" ")[1], CultureInfo.InvariantCulture),
                Link = x.GetAttribute("href")!.Trim().Replace("\t", "")
            }).ToList()
        };
        
        return comic;
    }

    public async Task<IEnumerable<Page>> GetChapter(string link)
    {
        var client = new RestClient(link);
        var request = new RestRequest();
        var response = await client.ExecuteAsync(request);
        if (!response.IsSuccessful || response.Content == null)
            throw new Exception("Error getting comic");
        
        var parser = new HtmlParser();
        var html = parser.ParseDocument(response.Content);
        
        var pages = html.QuerySelectorAll("#chapter_imgs > img");
        
        return pages.Select((x, index) => new Page
        {
            Link = x.GetAttribute("src")!.Trim().Replace("\t", ""),
            Number = index
        });
    }

    public async Task<byte[]> GetPage(string link)
    {
        var client = new RestClient(link);
        var request = new RestRequest();
        var response = await client.DownloadDataAsync(request);
        if (response == null)
            throw new Exception("Error getting comic");

        return response;
    }
}