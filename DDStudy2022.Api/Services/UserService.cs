using AutoMapper;
using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Users;
using DDStudy2022.Common.Exceptions;
using DDStudy2022.DAL;
using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDStudy2022.Api.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public UserService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<bool> CheckUserExist(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }

    public async Task AddAvatarToUser(Guid userId, MetadataModel meta, string filePath)
    {
        var user = await _context.Users.Include(x => x.Avatar).FirstOrDefaultAsync(x => x.Id == userId);
        if (user != null)
        {
            var avatar = new Avatar
            {
                Author = user,
                MimeType = meta.MimeType,
                FilePath = filePath,
                Name = meta.Name,
                Size = meta.Size
            };
            user.Avatar = avatar;

            await _context.SaveChangesAsync();
        }
    }

    public async Task<AttachModel> GetUserAvatar(Guid userId)
    {
        var user = await GetUserById(userId);
        var atach = _mapper.Map<AttachModel>(user.Avatar);
        return atach;
    }

    public async Task<IEnumerable<UserAvatarModel>> GetUsers() =>
        await _context.Users.AsNoTracking()
            .Include(x => x.Avatar)
            .Include(x => x.Posts)
            .Select(x => _mapper.Map<UserAvatarModel>(x))
            .ToListAsync();


    public async Task<UserAvatarModel> GetUser(Guid id) =>
        _mapper.Map<User, UserAvatarModel>(await GetUserById(id));


    private async Task<User> GetUserById(Guid id)
    {
        var user = await _context.Users.Include(x => x.Avatar).Include(x => x.Posts)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (user == null || user == default)
            throw new UserException("Not found");
        return user;
    }
}