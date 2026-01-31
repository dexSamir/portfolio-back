
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Portfolio.Application;
using Portfolio.Domain.Entities;
using Portfolio.Infrastructure;
using Portfolio.Infrastructure.ServiceRegistrations;
using Portfolio.Persistence;
using Portfolio.Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Portfolio Api", Version = "v1" }); 
});

builder.Services.AddDbContext<PortfolioDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

builder.Services
    .AddIdentity<User, Role>(options =>
    {
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<PortfolioDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddPersistentServices();
builder.Services.AddApplication();
builder.Services.AddCache(); 
builder.Services.AddExternalServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
// app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.Run();