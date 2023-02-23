using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;

namespace ToDo.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.Filters.Add(typeof(Middleware.ReformatValidationProblemAttribute)); /* Change default exception object returned by the API */
                })
                .ConfigureApiBehaviorOptions(options => { options.SuppressMapClientErrors = true; })
                .AddNewtonsoftJson();

            /* Cors */
            services.AddCors();

            /* Sql connection */
            services.AddSingleton<DbAccessLibrary.ISqlFactory, DbAccessLibrary.SqlFactory>();
            services.AddTransient<DbAccessLibrary.IToDoDatabase, DbAccessLibrary.SqLite.ToDoDatabase>();

            /* Fluent validation */
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Models.Api.SimpleError>();
            services.AddTransient<IValidatorInterceptor, Middleware.ValidatorInterceptor>();

            /* Add Mediatr */
            services.AddMediatR((configuration) =>
            {
                configuration.RegisterServicesFromAssembly(Services.LibraryAssembly.Value);
                configuration.Lifetime = ServiceLifetime.Scoped;
            });

            /* Add swagger */
            services.AddSwaggerGen(options => 
            {
                options.CustomSchemaIds(type => type.ToString());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x.AllowAnyMethod()
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .SetIsOriginAllowed(origin => true)
                              .AllowCredentials());

            app.UseSwagger();
            app.UseSwaggerUI(Options => {
                Options.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo Api");
                Options.InjectStylesheet("/swagger-ui/custom.css");
                Options.RoutePrefix = string.Empty;
                Options.DefaultModelsExpandDepth(-1);
            });

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
