using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NekoDownloader.Core.Configs;
using NekoDownloader.Core.Interfaces;
using NekoDownloader.Core.Interfaces.Repositories;
using NekoDownloader.Core.Interfaces.Services;
using NekoDownloader.Infrastructure.Data;
using NekoDownloader.Infrastructure.Repositories;
using NekoDownloader.Services;
using NekoDownloader.Services.PeriodicHosted;
using NekoDownloader.Services.Schedules;

var builder = WebApplication.CreateBuilder(args);

// Add Settings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Add DB Entities Repositories
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(IComicRepository), typeof(ComicRepository));
builder.Services.AddScoped(typeof(IChapterRepository), typeof(ChapterRepository));
builder.Services.AddScoped(typeof(IPageRepository), typeof(PageRepository));

// Add Services
builder.Services.AddScoped<IComicService, ComicService>();
builder.Services.AddScoped<IPageService, PageService>();

// Add Periodic Service
builder.Services.AddScoped<SyncComicSchedule>();
builder.Services.AddScoped<SyncChapterSchedule>();
builder.Services.AddScoped<SyncPageSchedule>();

// Add Periodic Hosted Service
builder.Services.AddSingleton<PeriodicHostedService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<PeriodicHostedService>());

// Add DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
        x =>
        {
            x.MigrationsAssembly("NekoDownloader.Infrastructure");
            x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }));

// Add Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Allow CORS
const string corsDevelop = "_CORSDevelop";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsDevelop,
        policy  =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options => 
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(corsDevelop);

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.MapControllers();

app.Run();
