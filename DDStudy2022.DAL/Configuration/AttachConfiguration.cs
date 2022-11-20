using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDStudy2022.DAL.Configuration;

public class AttachConfiguration : IEntityTypeConfiguration<Attach>
{
    public void Configure(EntityTypeBuilder<Attach> builder)
    {
        builder.ToTable("Attaches");

        builder.HasKey(it => it.Id);
    }
}