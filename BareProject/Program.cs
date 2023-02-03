using System.Text.Json.Serialization;
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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions
    .ReferenceHandler = ReferenceHandler.Preserve);;
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var siteSettings = builder.Configuration.GetSection("SiteSettings").Get<SiteSettings>();
builder.Services.AddDbContext(builder);
builder.Services.AddCustomIdentity(siteSettings.IdentitySettings);
builder.Services.AddScoped<IBusinessLogic, BusinessLogic>();
builder.Services.AddScoped<IRepository<RawMaterial>, Repository<RawMaterial>>();
builder.Services.AddScoped<IRepository<Service>, Repository<Service>>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddCustomApiVersioning();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<CustomExceptionHandlerMiddleware>();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
});
app.Run();