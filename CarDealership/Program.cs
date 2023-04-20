using System.Net;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common;
using Data;
using Data.Contracts;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using Services;
using WebFramework.Configuration;
using WebFramework.Middlewares;
using WebFramework.Swagger;
using WebFramwork.Configuration;
using WebFramwork.CustomMapping;

var builder = WebApplication.CreateBuilder(args);
//NLOG & Sentry
#region Sentry/NLog

//We used NLog.Targets.Sentry2 library formerly
//But it was not based on NetStandard and used an unstable SharpRaven library
//So we decided to replace it with a better library
//The NLog.Targets.Sentry3 library supports NetStandard2.0 and uses an updated version of SharpRaven library.
//But Sentry.NLog is the official sentry library integrated with nlog and better than all others.

//NLog.Targets.Sentry3
//https://github.com/CurtisInstruments/NLog.Targets.Sentry

//Sentry SDK for .NET
//https://github.com/getsentry/sentry-dotnet

//Sample integration of NLog with Sentry
//https://github.com/getsentry/sentry-dotnet/tree/master/samples/Sentry.Samples.NLog


//Set deafult proxy
WebRequest.DefaultWebProxy = new WebProxy("http://127.0.0.1:8118", true) { UseDefaultCredentials = true };

// You can configure your logger using a configuration file:

// If using an NLog.config xml file, NLog will load the configuration automatically Or, if using a
// different file, you can call the following to load it for you: 
//LogManager.Configuration = LogManager.LoadConfiguration("NLog-file.config").Configuration;



// Or you can configure it with code:
//UsingCodeConfiguration();

#endregion
// Add services to the container.

builder.Services.AddControllers();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureLogging(options => options.ClearProviders())
    .UseNLog()
    .ConfigureContainer<ContainerBuilder>(z =>
{
    z.AddServices();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(nameof(SiteSettings)));
builder.Services.InitializeAutoMapper();
var siteSettings = builder.Configuration.GetSection("SiteSettings").Get<SiteSettings>();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddCustomIdentity(siteSettings.IdentitySettings);
builder.Services.AddMinimalMvc();
builder.Services.AddJwtAuthentication(siteSettings.JwtSettings);
builder.Services.AddElmahCore(builder.Configuration, siteSettings);
builder.Services.AddCustomApiVersioning();
builder.Services.AddSwagger();
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});
var app = builder.Build();

app.IntializeDatabase();
app.UseCustomExceptionHandler();
app.UseHsts(app.Environment);
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(x=> x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200", "https://localhost:7163"));
app.UseSwaggerAndUi();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(config =>
{
    config.MapControllers(); // Map attribute routing
    //    .RequireAuthorization(); Apply AuthorizeFilter as global filter to all endpoints
    //config.MapDefaultControllerRoute(); // Map default route {controller=Home}/{action=Index}/{id?}
});
app.Run();