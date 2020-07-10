using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Matchbook.Business;
using Matchbook.Db;
using Matchbook.Db.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Matchbook.WebHost
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
            // TODO: This has to run only in Dev environments.
            services.AddHostedService<DatabaseSeedingService>();
            services.AddScoped<ILinkOrder, LinkOrder>();
            services.AddScoped<IOrdersDao, OrdersDao>();
            services.AddScoped<IOrderLinkDao, OrderLinkDao>();

            services.AddControllers();

            services.AddDbContext<MatchbookDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("MatchbookDb"), 
                    actions => actions.MigrationsAssembly(typeof(MatchbookDbContext).Assembly.FullName)));
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
