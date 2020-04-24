using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS.Db.Context;
using CS.Db.Models.Rest;
using CS.Service.RestApiNode;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CS.WebApi
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
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionStrings:default"]));
            services.AddSingleton(opt => new TransactionService(Configuration));
            services.AddSingleton(opt => new MonitorService(Configuration));
            services.AddSingleton(typeof(NodeAPIClient.Services.NodeInfoService));
            services.AddSingleton(instance => new ParseRequestService(Configuration));
            services.AddSingleton(typeof(BlocksService));
            services.AddOpenApiDocument();
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
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseDeveloperExceptionPage();
                app.UseHsts();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseCors(builder =>
                          builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            // Для миграции на проде
            //var contextDb = app.ApplicationServices.GetService<AppDbContext>();
            //contextDb.Database.Migrate();
        }
    }
}
