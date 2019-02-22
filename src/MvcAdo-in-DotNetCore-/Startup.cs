﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using MvcAdoDemo.Models;
using MVCAdoDemo.Models;
using Swashbuckle.AspNetCore.Swagger;


namespace MvcAdoDemo
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<AppDBContext>(pt=>pt.UseSqlServer(Configuration["ConnectionString:TrainingDB"]));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<IStudentIMDataAccessLayer,StudentIMDataAccessLayer>();            
            services.AddCors(opt =>{
                opt.AddPolicy("ngCrosPolicy",
                             Builder=>Builder
                                .WithOrigins("http://localhost:4200")
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                );
            }
            );
             //services.AddDbContext<AppIMDBContext>(opt => opt.UseInMemoryDatabase());

            //    var serviceProvider = new ServiceCollection()
            //         .AddEntityFrameworkInMemoryDatabase()
            //         .BuildServiceProvider();

                // Add a database context (AppDbContext) using an in-memory database for testing.
                // services.adddb               
                services.AddDbContext<AppIMDBContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryAppDb");
                    ///options.UseInternalServiceProvider(serviceProvider);
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Test API",
                    Description = "ASP.NET Core Web API"
                });
            });
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Shows UseCors with named policy. Globally
           // app.UseCros("ngCrosPolicy");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });
        }
    }
}

