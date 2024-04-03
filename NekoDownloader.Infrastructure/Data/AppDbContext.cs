using Microsoft.EntityFrameworkCore;
using NekoDownloader.Core.Entities;
using NekoDownloader.Infrastructure.Data.Configurations;

namespace NekoDownloader.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
     public DbSet<Comic> Comics { get; set; } = null!;
     public DbSet<Chapter> Chapters { get; set; } = null!;
     public DbSet<Page> Pages { get; set; } = null!;

     protected override void OnModelCreating(ModelBuilder builder)
     {
          builder.ApplyConfiguration(new ComicConfiguration());
          builder.ApplyConfiguration(new ChapterConfiguration());
          builder.ApplyConfiguration(new PageConfiguration());
     }
}