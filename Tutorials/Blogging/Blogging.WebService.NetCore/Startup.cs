using System.Diagnostics.Contracts;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Blogging.WebService
{
    public class Startup
    {
        #region Public Constructors
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Contract.Requires(configuration != null);
            Contract.Requires(webHostEnvironment != null);

            this.Configuration      = configuration;
            this.WebHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region Configure Methods
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson();

            services.AddHttpContextAccessor();

            services.AddApiServices(this.Configuration);
            services.AddBloggingRepository(this.Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        #endregion

        #region Private Properties
        private IConfiguration      Configuration      { get; }
        private IWebHostEnvironment WebHostEnvironment { get; }
        #endregion
    }
}
