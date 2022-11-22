using DDStudy2022.Api.Models.Users;

namespace DDStudy2022.Api.Models.Subscriptions;

public class SubscriptionModel
{
    public Guid Id { get; set; }
    public Guid SubUserId { get; set; }
    public Guid UserId { get; set; }
    public UserAvatarModel? User { get; set; }
    public UserAvatarModel? SubUser { get; set; }
}