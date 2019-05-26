using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoviesAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using MoviesAPI.Service;
using MoviesAPI.Repository;
using AutoMapper;
using MoviesAPI.ViewModels;
using MoviesAPI.Domain;
using MoviesAPI.Repository.Entities;

namespace MoviesAPI
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
            //Register config in DI
            //In Startup.ConfigureServices(), register custom configuration class in DI after being binded from config section
            var clientConfig = Configuration.GetSection("AppConfig")
                .Get<ApplicationSettings>();
            services.AddSingleton(clientConfig);
            services.AddSingleton<IConfiguration>(Configuration);

            //Register custom dbContext in DI
            services.AddDbContext<MoviesDbContext>(options =>
            {
                options.UseInMemoryDatabase("MoviesDb");
            });

            //Add interfaces implementations to DI
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IMovieRepository, MovieRepository>();

            services.AddAutoMapper();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
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

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }
    }

    static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MovieModel, MovieViewModel>()
                    .ForMember(m => m.Category, opt => opt.MapFrom(v => v.Category.Title));

                cfg.CreateMap<MovieEntity, MovieModel>()
                    .ForMember(m => m.Category, opt => opt.MapFrom(v => new CategoryModel() { Title = v.Category.Title }))
                    .ForMember(m => m.Title, opt => opt.MapFrom(v => v.Title))
                    .ForMember(m => m.ImageUrl, opt => opt.MapFrom(v => v.ImageUrl));
                    /* Ignore other unmapped members */ //.ForAllOtherMembers(opts => opts.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }
    }

}
