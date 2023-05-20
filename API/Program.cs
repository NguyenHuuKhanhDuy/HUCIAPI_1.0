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
});

#if DEBUG
var connectionString = builder.Configuration.GetConnectionString("HUCIDB_SERVER");
builder.Services.AddDbContext<HucidbContext>(options => options.UseSqlServer(connectionString));
#else
var connectionString = builder.Configuration.GetConnectionString("HUCIDB_SERVER");
builder.Services.AddDbContext<HucidbContext>(options => options.UseSqlServer(connectionString));
#endif

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add automapper
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);



// Add services to the container.
builder.Logging.ClearProviders(); 

builder.Services.TryAddEnumerable(
ServiceDescriptor.Singleton<ILoggerProvider, ColorConsoleLoggerProvider>());
LoggerProviderOptions.RegisterProviderOptions
<ColorConsoleLoggerConfiguration, ColorConsoleLoggerProvider>(builder.Services);

Log.Logger = new LoggerConfiguration()
           .WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day) // Configure File sink with daily rotation
           .CreateLogger();
builder.Services.AddLogging(configure => configure.AddSerilog());

//add scope
builder.Services.AddTransient<IEmployeeServices, EmployeeImp>();
builder.Services.AddTransient<IBrandServices, BrandImp>();
builder.Services.AddTransient<ICategoryServices, CategoryImp>();
builder.Services.AddTransient<IProductServices, ProductImp>();
builder.Services.AddTransient<ICustomerServices, CustomerImp>();
builder.Services.AddTransient<ISupplierServices, SupplierImp>();
builder.Services.AddTransient<IOrderServices, OrderImp>();
builder.Services.AddTransient<IPromotionServices, PromotionImp>();
builder.Services.AddTransient<ILocationServices, LocationImp>();
builder.Services.AddTransient<IFundServices, FundImp>();
builder.Services.AddTransient<ICommissionServices, CommissionImp>();
builder.Services.AddTransient<IOtherCostServices, OtherCostImp>();
builder.Services.AddTransient<ITimeKeepingServices, TimeKeepingImp>();
builder.Services.AddTransient<ICallTakeCareServices, CallTakeCareImp>();
builder.Services.AddTransient<IIPServices, IPImp>();
builder.Services.AddTransient<IShiftServices, ShiftImp>();
builder.Services.AddTransient<IOrderSourceServices, OrderSourceImp>();

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
