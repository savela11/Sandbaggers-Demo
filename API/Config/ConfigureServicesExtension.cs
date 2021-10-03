using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


namespace API.Config
{
    public static class ConfigureServicesExtension
    {
        public static void ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });
        }

        public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }

            public static bool Validate(
                DateTime? notBefore,
                DateTime? expires,
                SecurityToken tokenToValidate,
                TokenValidationParameters @param
            ) {
                return (expires != null && expires > DateTime.UtcNow);
            }
        public static void ConfigureJwtAuthentication(this IServiceCollection services, string secret)
        {
            var key = Encoding.ASCII.GetBytes(secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    LifetimeValidator = Validate,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        public static void ConfigureIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Stores.MaxLengthForKeys = 128;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureGlobalAuthorizationPolicy(this IServiceCollection services)
        {
            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        public static void ConfigureApiAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(o =>
            {
                o.AddPolicy("apiPolicy", b =>
                {
                    b.RequireAuthenticatedUser();
                    b.RequireClaim(ClaimTypes.Role, "Access.Api");
                    b.AuthenticationSchemes = new List<string> {JwtBearerDefaults.AuthenticationScheme};
                });
            });
        }


        // public static void AllConfigurations(this IServiceCollection services, string secret)
        // {
        //     ConfigureIdentityOptions(services);
        //     Services(services);
        //     Repositories(services);
        //     ConfigureApiAuthorization(services);
        //     ConfigureJwtAuthentication(services, secret);
        //     ConfigureIdentityService(services);
        //     ConfigureGlobalAuthorizationPolicy(services);
        // }
        //
        // public static void ConfigureIdentityOptions(IServiceCollection services)
        // {
        //     services.Configure<IdentityOptions>(options =>
        //     {
        //         options.User.RequireUniqueEmail = true;
        //         options.Password.RequireDigit = true;
        //         options.Password.RequireLowercase = false;
        //         options.Password.RequireNonAlphanumeric = false;
        //         options.Password.RequireUppercase = false;
        //         options.Password.RequiredLength = 6;
        //         options.Password.RequiredUniqueChars = 1;
        //
        //
        //     });
        // }
        //
        // public static void ConfigureJwtAuthentication(IServiceCollection services, string secret)
        // {
        //     var key = Encoding.ASCII.GetBytes(secret);
        //     services.AddAuthentication(x =>
        //     {
        //         x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //         x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //     }).AddJwtBearer(x =>
        //     {
        //         x.RequireHttpsMetadata = false;
        //         x.SaveToken = true;
        //
        //         x.TokenValidationParameters = new TokenValidationParameters
        //         {
        //             ValidateIssuerSigningKey = true,
        //             IssuerSigningKey = new SymmetricSecurityKey(key),
        //             ValidateIssuer = false,
        //             ValidateAudience = false,
        //             ValidateLifetime = true,
        //             ClockSkew = TimeSpan.Zero
        //         };
        //     });
        // }
        //
        // public static void ConfigureIdentityService(IServiceCollection services)
        // {
        //     services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        //     {
        //         options.SignIn.RequireConfirmedAccount = false;
        //         options.Stores.MaxLengthForKeys = 128;
        //     }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        // }
        //
        // public static void ConfigureGlobalAuthorizationPolicy(IServiceCollection services)
        // {
        //     services.AddMvc(o =>
        //     {
        //         var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        //         o.Filters.Add(new AuthorizeFilter(policy));
        //     });
        // }
        //
        // public static void ConfigureApiAuthorization(IServiceCollection services)
        // {
        //     services.AddAuthorization(o =>
        //     {
        //         o.AddPolicy("apiPolicy", b =>
        //         {
        //             b.RequireAuthenticatedUser();
        //             b.RequireClaim(ClaimTypes.Role, "Access.Api");
        //             b.AuthenticationSchemes = new List<string> {JwtBearerDefaults.AuthenticationScheme};
        //         });
        //     });
        // }
        //

    }
}
