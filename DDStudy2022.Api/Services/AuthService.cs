using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
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

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly AuthConfig _config;

    public AuthService(DataContext context, IMapper mapper, IOptions<AuthConfig> config)
    {
        _context = context;
        _mapper = mapper;
        _config = config.Value;
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

    private TokenModel GenerateTokens(UserSession session)
    {
        var dtNow = DateTime.Now;
        if (session.User == null)
            throw new UserException("Session not contains user");
        var jwt = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            notBefore: dtNow,
            claims: new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, session.User.Name!),
                new Claim("sessionId", session.Id.ToString()),
                new Claim("id", session.User.Id.ToString())
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
                new Claim("refreshToken", session.RefreshToken.ToString())
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

    private async Task<UserSession> GetSessionByRefreshToken(Guid id)
    {
        var session = await _context.Sessions.Include(x => x.User)
            .FirstOrDefaultAsync(x => x.RefreshToken == id);
        if (session == null)
        {
            throw new UserSessionException("Session is not found");
        }

        return session;
    }

    public async Task<TokenModel> Login(string login, string password)
    {
        var user = await GetUserByCredential(login, password);
        var session = await _context.Sessions.AddAsync(
            new UserSession
            {
                User = user,
                RefreshToken = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Id = Guid.NewGuid()
            });
        await _context.SaveChangesAsync();
        return GenerateTokens(session.Entity);
    }

    public async Task Registration(CreateUserModel model)
    {
        var dbUser = _mapper.Map<User>(model);
        await _context.Users.AddAsync(dbUser);
        await _context.SaveChangesAsync();
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

        if (principal.Claims.FirstOrDefault(x => x.Type == "refreshToken")?.Value is String refreshIdString
            && Guid.TryParse(refreshIdString, out var refreshId))
        {
            var session = await GetSessionByRefreshToken(refreshId);
            if (!session.IsActive)
            {
                throw new UserSessionException("Session is not active");
            }

            session.RefreshToken = Guid.NewGuid();
            await _context.SaveChangesAsync();

            return GenerateTokens(session);
        }

        throw new SecurityTokenException("invalid token");
    }
}