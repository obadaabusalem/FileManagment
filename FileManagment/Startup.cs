using FileManagment.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using Repositories;

namespace FileManagment
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
            //services.AddDbContextPool<ApplicationDbContext>(options => // NrsDBContext is the class we use to connect with sql
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString("MyConnection"));
            //});
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                string Connewction = Configuration.GetConnectionString("MyConnection");//Configuration.GetConnectionString("MyConnection");
                options.UseSqlServer(Connewction);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Folder Managment", Version = "v1" });
                //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            services.AddScoped<IFoldersRepository, FoldersRepository>();
            services.AddScoped<IFilesRepository, FilesRepository>();
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
            app.UseSwagger();
            app.UseSwaggerUI(c=> {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

           
        }
    }
}
