using Carrot.Contracts.Common;
using Carrot.Contracts.Services;
using Carrot.Entities;
using Carrot.Repositories;
using Carrot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;


namespace Carrot.API
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Carrot.API", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("ApiCors", builder =>
                {
                    builder.WithOrigins("https://localhost:8001", "http://localhost:8000");
                    builder.AllowCredentials();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });


            var inMemoryDatabase = Configuration.GetValue<bool>("InMemoryDatabase", false);
            if (inMemoryDatabase)
            {
                services.AddDbContext<Context>
                (options =>
                {
                    options.UseInMemoryDatabase("Gateway");
                });
            }
            else
            {
                services.AddDbContext<Context>(options =>
                    {
                        options.UseSqlServer(Configuration.GetConnectionString("Default"));
                    }
                );
            }

            services.AddScoped<RepositoryWrapper>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddSingleton<IOktaService, OktaService>();

            services.Configure<OktaConfig>(Configuration.GetSection("Okta"));
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Carrot.API v1"));
            //}

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Carrot.API v1"));

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("ApiCors");
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
