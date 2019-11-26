using Lamar;
using Microsoft.Extensions.Configuration;

namespace Document_Analyser_Common.Helpers
{
    public static class DependenciesHelper
    {
        public static T GetSettings<T>(IServiceContext context, string sectionName) where T : new()
        {
            var settings = new T();
            var configuration = context.GetInstance<IConfiguration>();

            configuration.GetSection(sectionName).Bind(settings);

            return settings;
        }

        public static T GetSettings<T>(IConfiguration configuration, string sectionName) where T : new()
        {
            var settings = new T();

            configuration.GetSection(sectionName).Bind(settings);

            return settings;
        }
    }
}
