using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDStudy2022.Api.Interfaces;
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

    private async Task<User> GetUserById(Guid id)
    {
        var user = await _context.Users.Include(it=>it.UserAccount).FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
            throw new UserException("User not found");
        return user;
    }

    public async Task DeleteUser(Guid id)
    {
        var dbUser = await GetUserById(id);
        _context.Users.Remove(dbUser);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckUserExistsByMail(string email)
    {
        var isExist = await _context.Users.AnyAsync(it => it.Email.ToLower() == email.ToLower());
        return isExist;
    }

    public async Task<UserModel> GetUser(Guid id)
    {
        var user = await GetUserById(id);
        return _mapper.Map<UserModel>(user);
    }

    public async Task<IEnumerable<UserModel>> GetAllUsers()
    {
        return await _context.Users.AsNoTracking()
            .ProjectTo<UserModel>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }


    public async Task<UserSession> GetSessionById(Guid id)
    {
        var session = await _context.Sessions.FirstOrDefaultAsync(x => x.Id == id);
        if (session == null)
        {
            throw new UserSessionException("Session is not found");
        }

        return session;
    }
}