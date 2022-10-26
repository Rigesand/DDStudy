using DDStudy2022.Api;
using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Services;
using DDStudy2022.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgresql"));
});
builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<ApiMappingProfile>(); });

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
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();