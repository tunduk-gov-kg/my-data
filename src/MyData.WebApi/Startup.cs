using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyData.Core.Interfaces;
using MyData.Core.Models;
using MyData.Infrastructure.EntityFrameworkCore;
using MyData.Infrastructure.Services;
using MyData.Infrastructure.XRoad;
using MyData.Infrastructure.XRoad.AutoMapper;

namespace MyData.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(typeof(XRoadProfile));

            services.AddDbContext<AppDbContext>(builder =>
                builder.UseNpgsql(Configuration.GetConnectionString("MyDataDb")));

            services.AddTransient<LogsCollectorService>();

            services.AddTransient<IJournalService, JournalService>();
            services.AddTransient<IXRoadLogsProcessor, XRoadLogsProcessor>();
            services.AddTransient<IXRoadRequestStore, XRoadRequestStore>();
            services.AddTransient<IXRoadServiceStore, XRoadServiceStore>();
            services.AddTransient<IXRoadDbReader, XRoadDbReader>();
            services.AddTransient<IXRoadLogsDbListProvider, InMemoryXRoadLogsDbListProvider>(Sample);
        }

        private InMemoryXRoadLogsDbListProvider Sample(IServiceProvider arg)
        {
            return new InMemoryXRoadLogsDbListProvider(new List<XRoadLogsDb>()
            {
                new XRoadLogsDb()
                {
                    Host = "localhost",
                    Port = 5432,
                    Database = "XRoadDb",
                    Username = "postgres",
                    Password = "postgres"
                }
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}