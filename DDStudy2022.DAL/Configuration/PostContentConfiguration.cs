using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDStudy2022.DAL.Configuration;

public class PostContentConfiguration: IEntityTypeConfiguration<PostContent>
{
    public void Configure(EntityTypeBuilder<PostContent> builder)
    {
        builder.ToTable("PostContents");
    }
}