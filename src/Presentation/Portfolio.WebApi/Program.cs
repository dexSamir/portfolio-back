
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Portfolio.Application;
using Portfolio.Domain.Entities;
using Portfolio.Infrastructure;
using Portfolio.Infrastructure.ServiceRegistrations;
using Portfolio.Persistence;
using Portfolio.Persistence.Contexts;
using Portfolio.WebAPI.Extensions;
using Portfolio.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(); 

builder.Services.AddDbContext<PortfolioDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

builder.Services
    .AddIdentity<User, IdentityRole<Guid>>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<PortfolioDbContext>()
    .AddDefaultTokenProviders();



builder.Services.AddPersistentServices();
builder.Services.AddApplication();
builder.Services.AddCache(); 
builder.Services.AddMapper();
builder.Services.AddExternalServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await AdminSeed.SeedAsync(scope.ServiceProvider);
}

app.UseGlobalExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "Portfolio API";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API v1");
        c.RoutePrefix = "swagger";
    });
}
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();