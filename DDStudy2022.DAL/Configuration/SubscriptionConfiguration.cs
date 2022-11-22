using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDStudy2022.DAL.Configuration;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasOne(it => it.User)
            .WithMany(it => it.Subscriptions)
            .HasForeignKey(it => it.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(it => it.SubUser)
            .WithMany(it => it.Subscribers)
            .HasForeignKey(it => it.SubUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}