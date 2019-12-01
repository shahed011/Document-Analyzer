using Amazon.Textract;
using Document_Analyzer_Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Lamar;
using Amazon.S3;
using Document_Analyzer_Services.Infrastructure.Dependencies;
using Microsoft.AspNetCore.Http;

namespace Document_Analyzer_Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ServiceRegistry services)
        {
            services.AddControllers();
            services.AddSingleton(Log.Logger);

            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonTextract>();

            services.IncludeRegistry<DocumentAnalyzerServiceRegistry>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger)
        {
            logger.Information(Configuration.Dump());
            logger.Information(Configuration.DumpEnvironmentVariables());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Map("/operations/status", configuration =>
            {
                configuration.Run(async context =>
                {
                    await context.Response.WriteAsync("IPMONITOROK");
                });
            });

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
