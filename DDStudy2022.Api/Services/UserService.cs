using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDStudy2022.Api.Configs;
using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Tokens;
using DDStudy2022.Api.Models.Users;
using DDStudy2022.Common.Exceptions;
using DDStudy2022.Common.Services;
using DDStudy2022.DAL;
using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DDStudy2022.Api.Services;

public class UserService : IUserService, ITokenService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly AuthConfig _config;

    public UserService(IMapper mapper,
        DataContext context,
        IOptions<AuthConfig> config
    )
    {
        _mapper = mapper;
        _context = context;
        _config = config.Value;
    }

    private async Task<User> GetUserById(Guid id)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
            throw new UserException("User not found");
        return user;
    }

    private async Task<User> GetUserByCredential(string login, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(it => it.Email!.ToLower() == login.ToLower());
        if (user == null)
            throw new UserException("User not found");
        if (!HashService.Verify(password, user.PasswordHash!))
            throw new UserException("Password is incorrect");
        return user;
    }

    private TokenModel GenerateTokens(User user)
    {
        var dtNow = DateTime.Now;
        var jwt = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            notBefore: dtNow,
            claims: new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name!),
                new Claim("id", user.Id.ToString())
            },
            expires: DateTime.Now.AddMinutes(_config.LifeTime),
            signingCredentials: new SigningCredentials(_config.SymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256)
        );
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var refresh = new JwtSecurityToken(
            notBefore: dtNow,
            claims: new[]
            {
                new Claim("id", user.Id.ToString()),
            },
            expires: DateTime.Now.AddHours(_config.LifeTime),
            signingCredentials: new SigningCredentials(_config.SymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256)
        );
        var encodedRefresh = new JwtSecurityTokenHandler().WriteToken(refresh);

        var tokens = new TokenModel
        {
            AccessToken = encodedJwt,
            RefreshToken = encodedRefresh
        };

        return tokens;
    }

    public async Task CreateUser(CreateUserModel model)
    {
        var dbUser = _mapper.Map<User>(model);
        await _context.Users.AddAsync(dbUser);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(UpdateUser user)
    {
        var dbUser = await GetUserById(user.Id);
        dbUser = _mapper.Map<User>(user);
        _context.Users.Update(dbUser);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(Guid id)
    {
        var dbUser = await GetUserById(id);
        _context.Users.Remove(dbUser);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> FindByMail(string email)
    {
        var isExist = await _context.Users.AnyAsync(it => it.Email == email);
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

    public async Task<TokenModel> Login(string login, string password)
    {
        var user = await GetUserByCredential(login, password);

        return GenerateTokens(user);
    }

    public async Task<TokenModel> GetTokenByRefreshToken(string refreshToken)
    {
        var validParams = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = _config.SymmetricSecurityKey()
        };
        var principal = new JwtSecurityTokenHandler().ValidateToken(refreshToken, validParams, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtToken
            || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("invalid token");
        }

        if (principal.Claims.FirstOrDefault(x => x.Type == "id")?.Value is String userIdString
            && Guid.TryParse(userIdString, out var userId))
        {
            var user = await GetUserById(userId);
            return GenerateTokens(user);
        }

        throw new SecurityTokenException("invalid token");
    }
}