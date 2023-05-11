using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace api
{
    public class Startup
    {
        private const string corsPolicy = "_allowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
             {
                 options.AddPolicy(name: corsPolicy,
                 builder =>
                 {
                     builder.WithOrigins("http://web", "http://localhost:8080", "http://localhost:8080/", "http://localhost:3500")
                     .AllowAnyHeader()
                     .AllowCredentials();
                 });
             });
            services.AddControllers();
            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("first", options =>
            {
                options.Authority = "https://dev-qeikcy1n4qn7rptk.us.auth0.com/";
                options.Audience = "https://SecKrcGenkAbbonees";

            }).AddJwtBearer("second", options =>
            {
                options.Authority = "https://dev-10d1syaroqa6fmth.us.auth0.com/";
                options.Audience = "http://localhost:5000";

            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(context =>
                    {
                        return context.User.HasClaim(c =>
                            (c.Type == "scope" &&
                            (c.Value.Contains("krc-genk"))
                            ));
                    });
                    policy.AddAuthenticationSchemes("first", "second");
                });
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(corsPolicy);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                .RequireAuthorization("ApiScope");
            });
        }
    }
}
