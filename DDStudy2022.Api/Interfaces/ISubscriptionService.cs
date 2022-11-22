using DDStudy2022.Api.Models.Subscriptions;

namespace DDStudy2022.Api.Interfaces;

public interface ISubscriptionService
{
    public Task Subscribe(SubscriptionRequest newSubscription, Guid userId);
    public Task UnSubscribe(SubscriptionRequest newSubscription, Guid userId);
}