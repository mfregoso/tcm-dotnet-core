using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TCM.Models.Entities;

namespace TCM
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            InDevelopment = env.IsDevelopment();
        }

        public IConfiguration Configuration { get; }
        public bool InDevelopment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            var db = InDevelopment ? "LocalDb" : "CloudDb";
            services.AddDbContext<ClubDataContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString(db), b => b.MigrationsAssembly("TCM.Web")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (InDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
