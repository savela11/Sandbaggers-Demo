using API.Config;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Utilities;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private string VueAppPolicy { get; set; } = "VueAppPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(VueAppPolicy, builder =>
                {
                    builder.WithOrigins(
                            "https://sandbaggersclient.z20.web.core.windows.net/")
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("PGSQLConnection")));
            // services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.Repositories();
            services.Services();
            services.ConfigureIdentityOptions();
            services.ConfigureGlobalAuthorizationPolicy();
            services.ConfigureIdentityService();
            services.ConfigureApiAuthorization();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    // policy.RequireClaim("Claim", "User");
                    policy.RequireRole("User");
                });
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    // policy.RequireClaim("Claim", "Admin");
                    policy.RequireRole("Admin");
                });
            });
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettingsExtension>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettingsExtension>();
            services.ConfigureJwtAuthentication(appSettings.Secret);
            services.AddControllers();
            services.AddSwaggerGen();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "API", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("VueAppPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    "{controller}/{action}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "areas",
                    "{area}/{controller}/{action}/{id?}"
                );
                // endpoints.MapControllers();
            });
        }
    }
}
