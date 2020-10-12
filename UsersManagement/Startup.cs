using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NLog;
using UsersManagement.Utility;

namespace UsersManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });
            services.AddControllersWithViews().SetCompatibilityVersion(CompatibilityVersion.Latest);

            //Provide a secret key to Encrypt and Decrypt the Token - JRozario
            var SecretKey = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);
            //Configure JWT Token Authentication - JRozario
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(token =>
            {
                token.RequireHttpsMetadata = false;
                token.SaveToken = true;
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(SecretKey),
                    ValidateIssuer = true,
                    //Usually this is your application base URL - JRozario
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    //Here we are creating and using JWT within the same application. In this case base URL is fine - JRozario
                    //If the JWT is created using a web service then this could be the consumer URL - JRozario
                    ValidAudience = Configuration["Jwt:Issuer"],
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddDbContext<UsersManagement.DAL.DatabaseEntity.UserManagementContext>(optionsAction: option => option.UseSqlServer(Configuration.GetConnectionString(name: "MyConn")), ServiceLifetime.Transient);
            services.AddScoped<BAL.Contracts.Users.IUsers, BAL.Users.Users>();
            // Inject unit of work
            services.AddScoped(typeof(Repository.Contracts.UnitOfWork.IUnitOfWork), typeof(Repository.Common.UnitOfWork));
            services.AddScoped(typeof(Repository.Contracts.Repository.IRepository<>), typeof(UsersManagement.Repository.Common.Repository<>));
            services.AddSingleton<HandleExceptionAttribute>();
            services.AddDataProtection();
            services.AddSingleton<Utility.Security.EncryptionService>();

            services.AddResponseCompression();

            services.Configure<GzipCompressionProviderOptions>(options => {
                options.Level = CompressionLevel.Fastest;
            });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseStatusCodePagesWithRedirects("/Home/Error");
            //app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //Addd User session - JRozario
            app.UseSession();

            //Add JWToken to all incoming HTTP Request Header - JRozario
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });
            //Add JWToken Authentication service - JRozario
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
        }
    }
}
