using Amazon.S3;
using Document_Analyser_Services.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Document_Analyser_Api.Extensions
{
    public static class AwsLocalstackExtension
    {
        //public static IServiceCollection AddAwsAndLocalStack(this IServiceCollection services, IConfiguration configuration, S3Settings s3Settings)
        //{
        //    if (s3Settings.LocalStackEnabled)
        //    {
        //        var url = new Uri(s3Settings.LocalStackEndpointUrl ?? string.Empty);
        //        services.AddSingleton<IAmazonS3>(new AmazonS3Client(new AmazonS3Config
        //        {
        //            ServiceURL = url.ToString(),
        //            UseHttp = true,
        //            ForcePathStyle = true,
        //            ProxyHost = url.Host,
        //            ProxyPort = url.Port
        //        }));
        //    }
        //    else
        //    {
        //        //services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        //        services.AddAWSService<IAmazonS3>();
        //    }

        //    return services;
        //}
    }
}
