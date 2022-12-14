using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Subscriptions;
using DDStudy2022.Common.Consts;
using DDStudy2022.Common.Exceptions;
using DDStudy2022.Common.Exstensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDStudy2022.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = "Api")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task Subscribe([FromBody] SubscriptionRequest request)
    {
        var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
        if (userId != default)
        {
            await _subscriptionService.Subscribe(request, userId);
        }
        else
            throw new UserException("you are not authorized");
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task UnSubscribe([FromBody] SubscriptionRequest request)
    {
        var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
        if (userId != default)
        {
            await _subscriptionService.UnSubscribe(request, userId);
        }
        else
            throw new UserException("you are not authorized");
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubscriptionModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<SubscriptionModel>> GetSubscription(Guid userId)
        => await _subscriptionService.GetSubscription(userId);

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubscriptionModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<SubscriptionModel>> GetSubscribers(Guid subUserId)
        => await _subscriptionService.GetSubscribers(subUserId);
}