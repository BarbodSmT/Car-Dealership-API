using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common;
using Data;
using Data.Contracts;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Services;
using WebFramework.Configuration;
using WebFramework.Middlewares;
using WebFramework.Swagger;
using WebFramwork.Configuration;
using WebFramwork.CustomMapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(z =>
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
builder.Services.AddCustomApiVersioning();
builder.Services.AddSwagger();
var app = builder.Build();

app.IntializeDatabase();
app.UseCustomExceptionHandler();
app.UseHsts(app.Environment);
app.UseHttpsRedirection();
app.UseRouting();
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