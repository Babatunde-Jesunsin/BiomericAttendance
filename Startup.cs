using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
using Microsoft.OpenApi.Models;
using Prunedge_User_Administration.Controllers;
using Prunedge_User_Administration.Data.Entities;
using Prunedge_User_Administration.Repository;
using Prunedge_User_Administration.Security;
using Prunedge_User_Administration_Library.Repository;
//using Prunedge_User_Administration.Services;

namespace Prunedge_User_Administration
{
    public class Startup
    {
        private readonly IConfigurationSection _jwtConfig;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _jwtConfig = configuration.GetSection("tokenManagement");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddDbContext<AdminstrationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<AdminstrationDbContext>()
                            .AddDefaultTokenProviders();
            services.AddControllers();
            //swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
           
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
              "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                 {
                          new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              },
                         Scheme = "oauth2",
                         Name = "Bearer",
                         In = ParameterLocation.Header,
                 },
                 new List<string>()
                 }
            });
            });


            // ===== Add Jwt Authentication ========
            services.AddScoped<Security.IJwtSecurity, JwtSecurity>();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.GetValue<string>("JwtKey")));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
                {
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                     
                    };


                });

            // var secret = Encoding.ASCII.GetBytes(token.Secret);
          //  services.AddTransient<IAuthService, AuthService>();
            services.AddScoped<IAuthRepo, AuthRepo>() ;
            services.AddScoped<ICourseRepo, CourseRepo>();
            // services.AddControllers(AuthController);
            // services.AddTransient<IAuthService, AuthService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            // ===== Use Authentication ======
            app.UseAuthentication();
           

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
