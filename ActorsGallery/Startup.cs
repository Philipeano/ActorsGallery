using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ActorsGallery.Data.Contracts;
using ActorsGallery.Data.MySqlDataService;
using ActorsGallery.Data.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ActorsGallery
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
            services.AddControllers(setupAction =>
            {
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                setupAction.Filters.Add(new ProducesAttribute("application/json"));
                setupAction.Filters.Add(new ConsumesAttribute("application/json"));
            });

            services.AddHttpContextAccessor();

            services
                .AddDbContextPool<ActorsGalleryContext>(options => options
                .UseMySQL(Configuration.GetConnectionString("HerokuClearDbConnStr")));

            services.AddScoped<ICharacterData, CharacterData>();
            services.AddScoped<IEpisodeData, EpisodeData>();
            services.AddScoped<ICommentData, CommentData>();
            services.AddScoped<ILocationData, LocationData>();
            services.AddScoped<IFetcher, Fetcher>();

            services.AddSwaggerGen(setupAction =>
            {
                // Create and configure OpenAPI specification document with basic information 
                setupAction.SwaggerDoc("ActorsGalleryOpenAPISpecs",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "ActorsGallery API",
                        Version = "1",
                        Description = "This API enables users fetch information about motion picture episodes and characters, and post comments. " +
                        "Please note that this application is a work in progress. " +
                        "Consequently, certain features, particularly 'authentication', 'update' and 'delete' operations, will be available in the next version of the API.",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Email = "philipeano@gmail.com",
                            Name = "Philip Newman",
                            Url = new Uri("https://www.twitter.com/philipeano")
                        },
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });

                //Fetch all XML output documents, and include their content in the OpenAPI specification
                var currentAssembly = Assembly.GetExecutingAssembly();
                var linkedAssemblies = currentAssembly.GetReferencedAssemblies();
                var fullAssemblyList = linkedAssemblies.Union(new AssemblyName[] { currentAssembly.GetName() });
                var xmlCommentFiles = fullAssemblyList
                    .Select(a => Path.Combine(AppContext.BaseDirectory, $"{a.Name}.xml"))
                    .Where(f => File.Exists(f))
                    .ToArray();

                foreach (string xmlFile in xmlCommentFiles)
                {
                    setupAction.IncludeXmlComments(xmlFile);
                }
            });
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    "/swagger/ActorsGalleryOpenAPISpecs/swagger.json",
                    "ActorsGallery API"
                    );
                setupAction.RoutePrefix = "";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
