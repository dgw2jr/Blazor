using BlazorApp50.Microservices.TrafficReport.Messages;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BlazorApp50.Microservices.TrafficReport
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlazorApp50.Microservices.TrafficReport", Version = "v1" });
            });

            services.AddMassTransit(c =>
            {
                c.SetKebabCaseEndpointNameFormatter();

                c.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("192.168.0.101", h =>
                    {
                        h.Username("user");
                        h.Password("BipyglxcSHK2");
                    });

                    cfg.ConfigureEndpoints(ctx);
                });

                c.AddConsumers(typeof(Program).Assembly);
                c.AddRequestClient<ITrafficReportCreatedMessage>();
            });

            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlazorApp50.Microservices.TrafficReport v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
