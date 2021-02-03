using AttributesAndHandlers.ASP.NET_Core_Identity_authentication;
using AttributesAndHandlers.Authorization;
using AttributesAndHandlers.AuthorizationPolicyProvider;
using AttributesAndHandlers.Data;
using AttributesAndHandlers.IJWTAuthentication;
using AttributesAndHandlers.JWTAuthentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AttributesAndHandlers
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
            services.AddDbContext<AppDbContext>(config =>
            {
                config.UseInMemoryDatabase("Memory");
            });

            services.AddIdentity<IdentityUser, IdentityRole>(x =>
            {
                x.Password.RequiredLength = 4;
                x.Password.RequireDigit = true;
                x.Password.RequireNonAlphanumeric = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            //services.ConfigureApplicationCookie(x =>
            //{
            //    x.Cookie.Name = "Mahyar.Cookie";
            //    x.LoginPath = "/api/Users/cookieAuth";
            //});

            //services.AddAuthentication("CookieAuth")
            //    .AddCookie("CookieAuth", config =>
            //     {
            //         config.Cookie.Name = "Mahyar.Cookie";
            //         config.LoginPath = "/api/Users/cookieAuth";
            //     });

            var tokenKey = Configuration.GetValue<string>("TokenKey");
            var key = Encoding.ASCII.GetBytes(tokenKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddSingleton<IJWTAuthenticationManager>(new JWTAuthenticationManager(tokenKey));

            //services.AddAuthorization(x =>
            //{
            //    var auth = new AuthorizationPolicyBuilder();
            //    var authPolicy = auth
            //    .RequireAuthenticatedUser()
            //    .RequireClaim(ClaimTypes.Role) //ForExample
            //    .Build();

            //    //x.DefaultPolicy = authPolicy;
            //    x.AddPolicy("Admin", role =>
            //        {
            //            role.RequireClaim(ClaimTypes.Role, "Admin");
            //            role.RequireRole(ClaimTypes.Role, "Admin");
            //            role.RequireRole("Admin"); // Role Base
            //        });
            //    x.AddPolicy("Claim.Mahyar", policy =>
            //      {
            //          policy.AddRequirements(new CustomRequireClaim(ClaimTypes.Name)); //Custom Claims for Policy Base
            //      });
            //});
            ////custom Claim By Identity
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Developer", policy =>
            //    policy.RequireClaim("IsDeveloper", "true"));
            //});

            //services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();

            //services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, ApplicationClaimsIdentityFactory>();

            //services.AddSingleton<IAuthorizationPolicyProvider, NamePolicyProvider>();

            //First approach for route
            var routeList = new List<string>() { "Get", "Post", "Put", "Patch" };
            services.AddAuthorization(x =>
            {
                x.AddPolicy("GetRoute", policy =>
                policy.RequireClaim("Routes", routeList[0]));
            });

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            services.AddTransient<IAuthorizationPolicyProvider, MinimumTimeSpendPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, MinimumTimeSpendHandler>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddAuthorization();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
