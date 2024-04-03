using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using NekoDownloader.Core.Entities;
using NekoDownloader.Core.Interfaces.Sources;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace NekoDownloader.Infrastructure.Sources;

public class IkigaiMangasSource : IBaseSource
{
    private const string ChaptersUrl = "https://panel.ikigaimangas.com/api/swf/series/{serie}/chapters";
    private const string PagesUrl = "https://ikigaimangas.com/capitulo/{chapter}/";
    private readonly Regex _patternPage = new (@"https:\/\/media\.ikigaimangas\.cloud\/series\/\d{1,}\/\d{1,}\/\d{1,}\.\w{1,}");
    
    public async Task<Comic> GetComic(string serie)
    {
        var client = new RestClient(ChaptersUrl.Replace("{serie}", serie));
        var request = new RestRequest();
        var response = await client.ExecuteAsync(request);
        if (!response.IsSuccessful || response.Content == null)
            throw new Exception("Error getting comic");

        var data = JObject.Parse(response.Content);
        
        var chapters = data["data"]!.Select(page => new
        {
            Name = (string)page["name"]!,
            Number = (float)page["name"]!,
            Url = (string)page["id"]!
        }).ToList();
        
        var nextPage = data["links"]!["next"]!;
        while (!nextPage.ToString().IsNullOrEmpty())
        {
            request = new RestRequest(nextPage.ToString());
            response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful || response.Content == null)
                throw new Exception("Error getting comic");

            data = JObject.Parse(response.Content);
            chapters.AddRange(data["data"]!.Select(page => new
            {
                Name = (string)page["name"]!,
                Number = (float)page["name"]!,
                Url = (string)page["id"]!
            }));
            nextPage = data["links"]!["next"]!;
        }
        
        Comic comic = new Comic
        {
            Chapters = chapters.Select(page => new Chapter
            {
                Title = page.Name,
                Number = page.Number,
                Link = page.Url
            }).ToList()
        };
        return comic;
    }

    public async Task<IEnumerable<Page>> GetChapter(string link)
    {
        var client = new RestClient(PagesUrl.Replace("{chapter}", link));
        var request = new RestRequest();
        var response = await client.ExecuteAsync(request);
        if (!response.IsSuccessful || response.Content == null)
            throw new Exception("Error getting comic");

        var body = response.Content!;
        
        var matches = _patternPage.Matches(body);
        var links = matches.Select(match => match.Value).Distinct().ToList();
        
        return links.Select((link, index) => new Page
        {
            Number = index,
            SourceLink = link
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