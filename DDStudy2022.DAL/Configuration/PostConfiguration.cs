using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDStudy2022.DAL.Configuration;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(it => it.Id);

        builder.HasMany(it => it.Comments)
            .WithOne(it => it.Post)
            .HasForeignKey(it => it.PostId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(it => it.Photos)
            .WithOne(it => it.Post)
            .HasForeignKey(it => it.PostId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}