using BrickendonDashboard.Api.Middleware;
using BrickendonDashboard.DbPersistence;
using BrickendonDashboard.Domain.Contexts;
using BrickendonDashboard.Domain.Contracts;
using BrickendonDashboard.Services;
using BrickendonDashboard.Shared.Contracts;
using BrickendonDashboard.Shared.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using static BrickendonDashboard.Api.Middleware.AuthenticationMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowOrigin",
    builder => builder.WithOrigins("http://localhost:3000")
    .AllowAnyMethod()
    .AllowAnyHeader()
    );
});
builder.Services.AddControllers();
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddScoped<IDateTimeService, DateTimeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<RequestContext>();
builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.AddHttpContextAccessor();
//builder.Services.AddAuthentication("CustomAuthenticationScheme").AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomAuthenticationScheme", null);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseRouting();
app.UseCors();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<CommonExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
