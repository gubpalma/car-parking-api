using Car.Parking.Domain;
using Car.Parking.Infrastructure.Pricing;
using Car.Parking.Infrastructure.Repository;
using Car.Parking.Infrastructure.Rules;
using Car.Parking.Infrastructure.Rules.Tax;
using Car.Parking.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Car.Parking.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                .AddTransient<IRepository<ParkingEntry>, ParkingEntryRepository>();

            services
                .AddTransient<IParkingPricer, ParkingPricer>()
                .AddTransient<IDefaultRateRule, HourlyRateRule>();

            services
                .AddTransient<IRateRule, EarlyBirdRateRule>()
                .AddTransient<IRateRule, NightRateRule>()
                .AddTransient<IRateRule, WeekendRateRule>();

            services
                .AddTransient<ITaxRule, GstTaxRule>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
