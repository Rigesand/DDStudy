using AspNetCoreRateLimit;
using DDStudy2022.Api.Configs;
using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Mapper;
using DDStudy2022.Api.Middlewares;
using DDStudy2022.Api.Middlewares.Extensions;
using DDStudy2022.Api.Models.Users;
using DDStudy2022.Api.Services;
using DDStudy2022.Api.Validators;
using DDStudy2022.DAL;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var authSection = builder.Configuration.GetSection(AuthConfig.Position);
var authConfig = authSection.Get<AuthConfig>();


builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<ClientRateLimitOptions>(builder.Configuration.GetSection("ClientRateLimiting"));
builder.Services.Configure<ClientRateLimitPolicies>(builder.Configuration.GetSection("ClientRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "Введите токен пользователя",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    c.SwaggerDoc("Auth", new OpenApiInfo {Title = "Auth"});
    c.SwaggerDoc("Api", new OpenApiInfo {Title = "Api"});
});

//Регистрация сервисов
builder.Services.Configure<AuthConfig>(authSection);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAttachService, AttachService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<LinkGeneratorService>();
//builder.Services.AddSingleton<DdosGuard>();


//Регистрация валидаторов
builder.Services.AddScoped<IValidator<CreateUserModel>, CreateUserValidator>();


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgresql"));
});
builder.Services.AddAutoMapper(typeof(ApiMappingProfile).Assembly);
builder.Services
    .AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authConfig.Issuer,
            ValidateAudience = true,
            ValidAudience = authConfig.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = authConfig.SymmetricSecurityKey(),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ValidAccessToken", p =>
    {
        p.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        p.RequireAuthenticatedUser();
    });
});

var app = builder.Build();

using (var serviceScope = ((IApplicationBuilder) app).ApplicationServices
       .GetService<IServiceScopeFactory>()?.CreateScope())
{
    if (serviceScope != null)
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
        context.Database.Migrate();
    }
}


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("Api/swagger.json", "Api");
    c.SwaggerEndpoint("Auth/swagger.json", "Auth");
});

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseClientRateLimiting();
app.UseAuthorization();
app.UseTokenValidator();
app.UseValidationException();
app.MapControllers();

app.Run();