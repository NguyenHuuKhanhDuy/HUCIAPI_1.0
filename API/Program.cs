using API.Controllers;
using ApplicationCore.AutoMapper;
using ApplicationCore.Helper.HandleException;
using ApplicationCore.Helper.Logger;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Services.Implement;
using Services.Interface;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory =  // the interjection
        BaseController.ValidateModelState;
}); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add automapper
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

//add Database
var connectionString = builder.Configuration.GetConnectionString("HUCIDB");
builder.Services.AddDbContext<HucidbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Logging.ClearProviders(); 

builder.Services.TryAddEnumerable(
ServiceDescriptor.Singleton<ILoggerProvider, ColorConsoleLoggerProvider>());
LoggerProviderOptions.RegisterProviderOptions
<ColorConsoleLoggerConfiguration, ColorConsoleLoggerProvider>(builder.Services);

//add scope
builder.Services.AddTransient<IEmployeeServices, EmployeeImp>();
builder.Services.AddTransient<IBrandServices, BrandImp>();
builder.Services.AddTransient<ICategoryServices, CategoryImp>();
builder.Services.AddTransient<IProductServices, ProductImp>();

//add authen services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

//services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.ConfigureCustomExceptionMiddleware();
app.UseCors("corsapp");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
