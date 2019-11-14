using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SIO = System.IO;

namespace TESTING_WEB_API_ASP
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddLogging();
            services.AddSwaggerGen();

            // Удаление устаревших временных файлов
            Helpers.HFile.tempAllClear(7);

            // Получение строк подключения к базам данных
            string mssql_enabled = Configuration.GetConnectionString("MsSql_Enabled");
            string mysql_enabled = Configuration.GetConnectionString("MySql_Enabled");

            // Выполнение подключения
            if (mssql_enabled == "true")
                services.AddDbContext<Contexts.DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MsSql")));
            else if (mysql_enabled == "true")
                services.AddDbContext<Contexts.DatabaseContext>(options => options.UseMySql(Configuration.GetConnectionString("MySql")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
