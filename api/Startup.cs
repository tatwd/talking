using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Talking.Api.Data;
using Talking.Api.Repository;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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

            services.Configure<MongoSettings>(options => {
                options.ConnectionString = Configuration["MongoSettings:ConnectionUrl"];
                options.DatabaseName = Configuration["MongoSettings:DatabaseName"];
            });

            services.AddTransient<ICommentRepository, CommentRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger(typeof(Startup));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors();
            }
            else
            {
                app.UseHsts();
            }

            // 请求头信息日志
            app.Use(dgate => {
                dgate += (ctx) => {
                    logger.LogInformation("{0} {1}",
                        getClientIp(ctx),
                        ctx.Request.Headers["User-Agent"]);
                    return Task.CompletedTask;
                };
                return dgate;
            });

            app.UseStatusCodePages(builder => {
                builder.Run(async ctx => {
                    // 处理 404 请求
                    if (ctx.Response.StatusCode == StatusCodes.Status404NotFound) {
                        ctx.Response.ContentType = "application/json; charset=utf-8";
                        await ctx.Response.WriteAsync("{\"error\": \"failed:not found your url\"}");
                    }
                });
            });

            // app.UseHttpsRedirection();
            app.UseMvc();
        }

        private string getClientIp(HttpContext ctx)
        {
            var ip = ctx.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
                ip = ctx.Connection.RemoteIpAddress.ToString();
            return ip;
        }
    }
}
