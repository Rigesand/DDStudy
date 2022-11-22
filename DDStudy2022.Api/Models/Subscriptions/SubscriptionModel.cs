namespace DDStudy2022.Api.Models.Subscriptions;

public class SubscriptionModel
{
    public Guid Id { get; set; }
    public Guid SubUserId { get; set; }
    public Guid UserId { get; set; }
}