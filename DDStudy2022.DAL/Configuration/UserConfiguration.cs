using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDStudy2022.DAL.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(it => it.Id);

        builder.HasMany(it => it.Sessions)
            .WithOne(it => it.User)
            .HasForeignKey(it => it.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}