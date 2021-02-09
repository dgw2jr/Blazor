using Blazored.Modal;
using Blazored.Toast;
using Core;
using MassTransit;
using Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Blazored.Modal;
using Messages;
using MassTransit.Azure.ServiceBus.Core.Configurators;
using System;
using Microsoft.EntityFrameworkCore;
using BlazorApp50.Microservices.Traffic.Messages;

namespace BlazorApp50
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var razorPagesBuilder = services.AddRazorPages();
            if (Env.IsDevelopment())
            {
                razorPagesBuilder.AddRazorRuntimeCompilation();
            }

            services.AddServerSideBlazor();

            services.AddDbContext<WeatherContext>(options => options.UseSqlServer(Configuration.GetConnectionString("WeatherContext")));
            services.AddBlazoredToast();
            services.AddBlazoredModal();
            services.AddScoped<TokenProvider>();

            services.AddMassTransit(x => {
                x.SetKebabCaseEndpointNameFormatter();

                //x.UsingRabbitMq((ctx, cfg) => {
                //    cfg.Host(Configuration.GetValue<string>("MassTransit:Host"), h =>
                //    {
                //        h.Username(Configuration.GetValue<string>("MassTransit:Username"));
                //        h.Password(Configuration.GetValue<string>("MassTransit:Password"));
                //    });                    

                //    cfg.ConfigureEndpoints(ctx);
                //});

                x.UsingAzureServiceBus((ctx, cfg) =>
                {
                    var settings = new HostSettings
                    {
                        ServiceUri = new Uri(Configuration.GetValue<string>("MassTransit:Host")),
                        TokenProvider = Microsoft.Azure.ServiceBus.Primitives.TokenProvider.CreateSharedAccessSignatureTokenProvider(Configuration.GetValue<string>("MassTransit:Username"), Configuration.GetValue<string>("MassTransit:Password"))
                    };

                    cfg.Host(settings);
                    cfg.ConfigureEndpoints(ctx);
                });

                x.AddRequestClient<GetWeatherReports>();
                x.AddRequestClient<IGetTrafficReportsMessage>();
            });
            services.AddMediator();
            services.AddMassTransitHostedService();
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

            //app.ApplicationServices.GetService<IBusControl>().Start();
        }
    }
}
