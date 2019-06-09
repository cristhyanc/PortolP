using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Portol.Calculator.Delivery;
using Portol.Calculator.Map;
using Portol.Calculator.Payment;
using Portol.Common.Interfaces;
using Portol.Common.Interfaces.PortolWeb;
using PortolWeb.API.Helper;
using PortolWeb.Core.DeliveryServices;
using PortolWeb.Core.SmsServices;
using PortolWeb.Core.UserServices;
using PortolWeb.DA;
using PortolWeb.Entities;
using Serilog;
using Sinch.ServerSdk.Messaging;

namespace PortolWeb.API
{
    public class Startup
    {
        //tes
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddDbContext<DataContext>(x => x.UseSqlServer(appSettings.ConnectionString));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<ICustomerService>();
                        var userId = Guid.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
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

            var smsApi = Sinch.ServerSdk.SinchFactory.CreateApiFactory(appSettings.SinchAppKey, appSettings.SinchAppSecret).CreateSmsApi();

            services.AddSingleton<AppSettings>(appSettings);

            services.AddScoped<ISmsApi>(x => Sinch.ServerSdk.SinchFactory.CreateApiFactory(appSettings.SinchAppKey, appSettings.SinchAppSecret).CreateSmsApi());
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IDatabaseManagement, DatabaseManagement>();
            services.AddScoped<IDataContext, DataContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();            
            services.AddScoped<IImageManager, ImageManager>();
            services.AddScoped<IDeliveryCalculator, DeliveryCalculator>();
            services.AddScoped<IPaymentService>(x => new PaymentService("sk_test_RCIYxJRaaXpKMgOIJnAvCrle00HTrJc29p", "pk_test_anYBo8LB4sisfeaXq8VvJOOJ00z6gDKo7R"));
            services.AddScoped<IMapService>(x => new MapService("AjPaETRxkyP3rSDJ7vu2nce9mlY66bgZu0DvY_eIVpeSM5PES53q_9IGzOrxahcL"));
            

            // loggerFactory.AddSerilog();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseStaticFiles(); // For the wwwroot folder

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Images")),
                RequestPath = "/Images"
            });
        }
    }
}
