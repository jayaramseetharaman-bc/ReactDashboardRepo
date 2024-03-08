using BrickendonDashboard.Api.Middleware;
using BrickendonDashboard.DbPersistence;
using BrickendonDashboard.Domain.Contexts;
using BrickendonDashboard.Domain.Contracts;
using BrickendonDashboard.Domain.Dtos;
using BrickendonDashboard.Services;
using BrickendonDashboard.Shared.Contracts;
using BrickendonDashboard.Shared.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using static BrickendonDashboard.Domain.Dtos.ApplicationConfigurationInfo;


var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
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
builder.Services.AddScoped<IRolesService, RoleService>();
builder.Services.AddScoped<RequestContext>();
builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	// Add any required security definitions
	c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
	{
		In = ParameterLocation.Header,
		Name = "X-API-KEY",
		Type = SecuritySchemeType.ApiKey,
	});
	var openApiSecuritySchema = new OpenApiSecurityScheme()
	{
		Reference = new OpenApiReference
		{
			Type = ReferenceType.SecurityScheme,
			Id = "ApiKey"
		},
		In = ParameterLocation.Header
	};
	var openApiSecurityRequirement = new OpenApiSecurityRequirement
				{
					 { openApiSecuritySchema, new List<string>() }
				};
	c.AddSecurityRequirement(openApiSecurityRequirement);
});
builder.Services.AddSingleton(s =>
{
  ApplicationConfigurationInfo appConfigInfo = new ApplicationConfigurationInfo()
  {
    ApiKey = configuration["ApiKey"].ToString(),
    JwtTokenValidationInfo = new JwtTokenValidationConfigurationInfo()
    {
      Audience = configuration["AzureAd:Audience"],
      Issuer = configuration["AzureAd:Issuer"]
    }

  };
  return appConfigInfo;
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseMiddleware<CommonExceptionHandlerMiddleware>();
app.UseMiddleware<AuthenticatorMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
