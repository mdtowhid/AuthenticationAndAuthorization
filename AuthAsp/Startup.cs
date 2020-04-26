using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Identity;
using AuthAsp.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace AuthAsp
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
            services.AddDbContextPool<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EmployeeDbConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            services.AddTransient<IEmployeeRepository, SQLEmployeeRepository>();

            services.AddMvc(config =>
            {
                config.EnableEndpointRouting = false;
            });

            services.AddAuthorization(option => 
                option.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role")));


            /*
             :::CONFIGURE THIS IF AUTHORIZATION IS REQUIRED TO SET GLOBALLY!
             */
            //services.AddMvc(config => {
            //    var policy = new AuthorizationPolicyBuilder()
            //                    .RequireAuthenticatedUser().
            //                    Build();
            //    config.Filters.Add(new AuthorizeFilter(policy));
            //    config.EnableEndpointRouting = false;
            //});
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
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}