using AutoMapper;
using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Subscriptions;
using DDStudy2022.Common.Exceptions;
using DDStudy2022.DAL;
using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDStudy2022.Api.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public SubscriptionService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task Subscribe(SubscriptionRequest newSubscription, Guid userId)
    {
        var user = await _context.Users.Include(it => it.Subscriptions)
            .FirstOrDefaultAsync(it => it.Id == userId);
        if (user == null)
        {
            throw new UserException("User not found");
        }

        var sub = user.Subscriptions!.FirstOrDefault(it => it.SubUserId == newSubscription.SubUserId);
        if (sub != null)
        {
            throw new SubscriptionException("You have already subscribed");
        }

        var subDb = _mapper.Map<Subscription>(newSubscription);
        subDb.UserId = userId;

        await _context.Subscriptions.AddAsync(subDb);
        await _context.SaveChangesAsync();
    }

    public async Task UnSubscribe(SubscriptionRequest newSubscription, Guid userId)
    {
        var sub = await _context.Subscriptions
            .FirstOrDefaultAsync(it => it.UserId == userId && it.SubUserId == newSubscription.SubUserId);
        if (sub == null)
        {
            throw new SubscriptionException("You have already unsubscribed");
        }

        _context.Subscriptions.Remove(sub);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<SubscriptionModel>> GetSubscription(Guid userId)
    {
        var subscriptions = await _context.Subscriptions
            .Where(it => it.UserId == userId)
            .Select(x => _mapper.Map<SubscriptionModel>(x))
            .ToListAsync();
        if (subscriptions == null)
            throw new SubscriptionException("Subscription not Found");

        return subscriptions;
    }
    public async Task<IEnumerable<SubscriptionModel>> GetSubscribers(Guid subUserId)
    {
        var subscriptions = await _context.Subscriptions
            .Where(it => it.SubUserId == subUserId)
            .Select(x => _mapper.Map<SubscriptionModel>(x))
            .ToListAsync();
        if (subscriptions == null)
            throw new SubscriptionException("Subscription not Found");

        return subscriptions;
    }
}