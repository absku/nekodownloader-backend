using System.Globalization;
using AngleSharp.Html.Parser;
using NekoDownloader.Core.Entities;
using NekoDownloader.Core.Interfaces.Sources;
using RestSharp;

namespace NekoDownloader.Infrastructure.Sources;

public class LectorknsSource : IBaseSource
{
    private const string ChaptersUrl = "https://lectorkns.com/sr/{serie}";
    
    public async Task<Comic> GetComic(string serie)
    {
        var client = new RestClient(ChaptersUrl.Replace("{serie}", serie));
        var request = new RestRequest();
        var response = await client.ExecuteAsync(request);
        if (!response.IsSuccessful || response.Content == null)
            throw new Exception("Error getting comic");

        var parser = new HtmlParser();
        var html = parser.ParseDocument(response.Content);

        var chapters = html.QuerySelectorAll("li.wp-manga-chapter > a");
        var comic = new Comic
        {
            Chapters = chapters.Select(x => new Chapter
            {
                Title = x.TextContent,
                Number = float.Parse(x.TextContent.Split(" ")[1], CultureInfo.InvariantCulture),
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
        
        var pages = html.QuerySelectorAll("img.wp-manga-chapter-img");
        
        return pages.Select(x => new Page
        {
            SourceLink = x.GetAttribute("src")!.Trim().Replace("\t", ""),
            Number = int.Parse(x.GetAttribute("id")!.Split("-")[1], CultureInfo.InvariantCulture)
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