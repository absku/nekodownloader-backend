using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NekoDownloader.Core.Entities;
using NekoDownloader.Core.Enums;

namespace NekoDownloader.Infrastructure.Data.Configurations;

public class ComicConfiguration : IEntityTypeConfiguration<Comic>
{
    public void Configure(EntityTypeBuilder<Comic> builder)
    {
        builder.HasKey(x => x.Uuid);

        builder.Property(x => x.Uuid)
            .ValueGeneratedNever();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Resume)
            .IsRequired(false)
            .HasMaxLength(4096);

        builder.Property(x => x.Link)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<ComicType>());

        builder.Property(x => x.State)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<StateType>());
        
        builder.Property(x => x.Source)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<SourceType>());

        builder.Property(x => x.Genres)
            .IsRequired()
            .HasConversion(
                v => string.Join(',', v.Select(x => x.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Enum.Parse<GenreType>).ToList(),
                new ValueComparer<ICollection<GenreType>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                )
            );

        builder
            .ToTable("Comics");
    }
}