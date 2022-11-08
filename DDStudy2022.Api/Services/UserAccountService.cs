using AutoMapper;
using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Common.Exceptions;
using DDStudy2022.DAL;
using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDStudy2022.Api.Services;

public class UserAccountService : IUserAccountService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserAccountService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddAvatarToUser(Guid userAccountId, MetadataModel meta, string filePath)
    {
        var userAccount = await _context.UserAccounts.Include(x => x.Avatar)
            .FirstOrDefaultAsync(x => x.Id == userAccountId);
        if (userAccount == null)
        {
            throw new UserException("User not found");
        }

        var avatar = new Avatar
        {
            UserAccount = userAccount,
            MimeType = meta.MimeType,
            FilePath = filePath,
            Name = meta.Name,
            Size = meta.Size
        };
        userAccount.Avatar = avatar;

        await _context.SaveChangesAsync();
    }

    public async Task<AttachModel> GetUserAvatar(Guid userAccountId)
    {
        var avatar = await _context.Avatars.FirstOrDefaultAsync(it => it.UserAccountId == userAccountId);
        var atach = _mapper.Map<AttachModel>(avatar);
        return atach;
    }
}