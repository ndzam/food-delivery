using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryWebApi.Models;
using FoodDeliveryWebApi.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System;
using FoodDeliveryWebApi.Configs;
using Microsoft.AspNetCore.Authentication.Negotiate;
using FoodDeliveryWebApi.AuthorizationHandlers;

namespace FoodDeliveryWebApi
{
    public class Startup
    {

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

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
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
    
                                  });
            });

            services.Configure<APIConfigs>(Configuration.GetSection(
                                        "APIConfigs"));
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
            });
            Console.WriteLine(defaultApp.Name);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme
                    = "FirebaseToken";
            })
            .AddScheme<ValidateAuthenticationSchemeOptions, FirebaseAuthenticationHandler>
                    ("FirebaseToken", op => { });



            services.AddTransient<IFirebaseService, FirebaseService>();
            
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRestaurantService, RestaurantService>();
            services.AddControllers();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
