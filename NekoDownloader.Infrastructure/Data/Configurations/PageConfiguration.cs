using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NekoDownloader.Core.Entities;

namespace NekoDownloader.Infrastructure.Data.Configurations;

public class PageConfiguration : IEntityTypeConfiguration<Page>
{
    public void Configure(EntityTypeBuilder<Page> builder)
    {
        builder.HasKey(x => x.Uuid);

        builder.Property(x => x.Uuid)
            .ValueGeneratedNever();

        builder.Property(x => x.Number)
            .IsRequired();
        
        builder.Property(x => x.SourceLink)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(x => x.Link)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(x => x.Image)
            .IsRequired(false)
            .HasMaxLength(2048);

        builder.Property(x => x.ChapterUuid)
            .IsRequired();
        
        builder.HasOne(p=> p.Chapter)
            .WithMany(c => c.Pages)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .ToTable("Pages");
    }
}