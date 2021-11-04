using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Microsoft.IdentityModel.Tokens;
using SecureAPI.Models;
using SecureAPI.Services;
using SecureAPI_Identity_EF.Models;

namespace SecureAPI_Identity_EF
{
    public class Startup
    {
        // CORS origin policy name
        private readonly string _corsOriginName = "myCorsOrigin";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string defaultConnectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AuthUserContext>(opt => opt.UseSqlServer(defaultConnectionString));
            services.AddIdentity<AuthUser, IdentityRole>(opt => {}).AddEntityFrameworkStores<AuthUserContext>();
            
            services.AddControllers();

            // Configure CORS origin policy
            services.AddCors(opt => {
                opt.AddPolicy(_corsOriginName, builder => {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });

            // JWT Authentication
            var key = Encoding.ASCII.GetBytes(Configuration["JWTConfig:Key"]);
            var issuer = Configuration["JWTConfig:Issuer"];
            var audience = Configuration["JWTConfig:Audience"];

            services.Configure<JwtConfig>(Configuration.GetSection("JWTConfig"));
            services.AddScoped<IAuthService, AuthService>();

            services
                .AddAuthentication(auth => {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwt => {
                    jwt.RequireHttpsMetadata = false;
                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        RequireExpirationTime = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(_corsOriginName);
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
