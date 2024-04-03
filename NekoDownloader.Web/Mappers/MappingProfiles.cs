using AutoMapper;
using NekoDownloader.Core.Entities;
using NekoDownloader.Web.Models;
using NekoDownloader.Web.Models.SaveModel;

namespace NekoDownloader.Web.Mappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Comic, ComicModel>();
        CreateMap<Chapter, ChapterModel>();
        CreateMap<Chapter, ChapterWithPagesModel>();
        CreateMap<Page, PageModel>();
        
        CreateMap<ComicSaveModel, Comic>();
    }
}