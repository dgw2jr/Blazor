using BlazorApp50.Data;
using Blazored.Toast;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp50
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            services.AddDbContext<WeatherContext>(options => options.UseSqlServer(Configuration.GetConnectionString("WeatherContext")));
            services.AddBlazoredToast();
            services.AddScoped<TokenProvider>();

            services.AddMassTransit(x => {
                x.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host("192.168.0.88", h =>
                    {
                        h.Username("user");
                        h.Password("BipyglxcSHK2");
                    });

                    //cfg.ReceiveEndpoint("weather-report-created", e =>
                    //{
                    //    e.Handler<WeatherReportCreated>(async context =>
                    //    {
                    //        await Console.Out.WriteLineAsync($"Report Received: {context.Message.Summary}");
                    //    });
                    //});
                });
            });
            services.AddMassTransitHostedService();
            services.AddMediator();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
