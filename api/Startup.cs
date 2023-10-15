using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Talking.Api.Models;
using Talking.Domain.Data;
using Talking.Domain.Repository;
using Microsoft.Extensions.Hosting;

namespace Talking.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.Configure<MvcOptions>(opt =>
            {
                opt.EnableEndpointRouting = false;
            });

#if DEBUG
            services.AddCors(options => {
                options.AddPolicy(options.DefaultPolicyName,
                    builder => {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });
#endif

            services.AddSingleton<TalkingDbContext>();
            services.AddScoped<ICommentRepository, CommentRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors();
            }

            app.UseExceptionHandler(_ => _.Run(async ctx =>
            {
                var logger = loggerFactory.CreateLogger(typeof(Startup));
                var feature = ctx.Features.Get<IExceptionHandlerPathFeature>();
                var exception = feature.Error;
                logger.LogError(exception, "未处理异常");
                var detail = env.IsDevelopment() ? exception.ToString() : exception.Message;
                var ko = HttpResponseFactory.CreateKo(
                    code: 5,
                    message: "exception",
                    detail);
                ctx.Response.ContentType = "application/json; charset=utf-8";
                await ctx.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(ko));
            }));

            // app.UseHttpsRedirection();
            app.UseMvc();

            // app.UseRouting();
        }
    }
}
