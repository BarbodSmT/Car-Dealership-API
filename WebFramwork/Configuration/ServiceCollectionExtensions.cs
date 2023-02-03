using Common;
using Common.Exceptions;
using Common.Utilities;
using Data;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace WebFramework.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDbContext(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
            });
        }


        public static void AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(2, 1);
                o.ReportApiVersions = true;
                o.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"));
            });
            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
        }
    }
}