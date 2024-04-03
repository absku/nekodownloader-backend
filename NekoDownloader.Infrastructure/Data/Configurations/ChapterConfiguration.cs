using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NekoDownloader.Core.Entities;

namespace NekoDownloader.Infrastructure.Data.Configurations;

public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.HasKey(x => x.Uuid);

        builder.Property(x => x.Uuid)
            .ValueGeneratedNever();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Link)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Number)
            .IsRequired();

        builder.Property(x => x.ComicUuid)
            .IsRequired();
        
        builder.HasOne(c => c.Comic)
            .WithMany(c => c.Chapters)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .ToTable("Chapters");
    }
}