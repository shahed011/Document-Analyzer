using Amazon.S3.Transfer;
using Amazon.Textract;
using Document_Analyser_Api.Extensions;
using Document_Analyser_Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Lamar;
using Amazon.S3;
using Document_Analyser_Services.Infrastructure.Dependencies;
using Document_Analyser_Services.Infrastructure.Configuration;

namespace Document_Analyser_Api
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

            services.IncludeRegistry<DocumentAnalyserServiceRegistry>();
        }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddControllers();
        //    services.AddSingleton(Log.Logger);

        //    var s3Settings = Configuration.GetSection("S3Settings").Get<S3Settings>();
        //    services.AddSingleton(s3Settings);

        //    services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
        //    services.AddAWSService<IAmazonS3>();
        //    services.AddAWSService<IAmazonTextract>();

        //    services.AddSingleton<IFileService, FileService>();
        //    services.AddSingleton<ITextractTextAnalysisService, TextractTextAnalysisService>();
        //    services.AddSingleton<ITextractTextDetectionService, TextractTextDetectionService>();
        //    services.AddSingleton<IDocumentReadAnalyzeService, DocumentReadAnalyzeService>();
        //    services.AddSingleton<ITransferUtility, TransferUtility>();
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger)
        {
            logger.Information(Configuration.Dump());
            logger.Information(Configuration.DumpEnvironmentVariables());

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
        }
    }
}
