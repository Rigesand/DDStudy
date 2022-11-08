using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDStudy2022.DAL.Configuration;

public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.ToTable("UserAccounts");

        builder.HasKey(it => it.Id);

        builder.HasMany(it => it.Posts)
            .WithOne(it => it.UserAccount)
            .HasForeignKey(it => it.UserAccountId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(it => it.Comments)
            .WithOne(it => it.UserAccount)
            .HasForeignKey(it => it.UserAccountId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(it=>it.Avatar)
            .WithOne(it => it.UserAccount)
            .HasForeignKey<Avatar>(it => it.UserAccountId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}