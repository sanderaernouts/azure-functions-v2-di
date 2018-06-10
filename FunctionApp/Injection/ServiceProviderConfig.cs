using System;
using System.Reflection;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FunctionApp.Injection
{

    public static class ServiceProviderConfig
    {
        public const string LogCategoryHost = "Host";
        public const string LogCategoryFunction = "Function";
        public const string LogCategoryWorker = "Worker";
        public const string LogCategoryCustom = "FunctionApp";
        
        public static IServiceProvider Configure(ExtensionConfigContext context)
        {
            
            var configBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            var serviceCollection = new ServiceCollection();
            
            var factory = context.Config.LoggerFactory;

            if (factory == null)
            {
                throw new InvalidOperationException($"{nameof(context.Config.LoggerFactory)} in {nameof(ExtensionConfigContext)} cannot be null. Unable to refresh log filters");
            }

            var refreshFilters = factory.GetType().GetMethod("RefreshFilters", BindingFlags.NonPublic | BindingFlags.Instance);

            if (refreshFilters == null)
            {
                throw new InvalidOperationException(
                    $"Could not load private method RefreshFilters(..) from {factory.GetType().FullName}");
            }

            var filterOptions = new LoggerFilterOptions();
            filterOptions.AddFilter((category, level) =>
            {
                return category.StartsWith($"{LogCategoryHost}.") ||
                       category.StartsWith($"{LogCategoryFunction}.") ||
                       category.StartsWith($"{LogCategoryWorker}.") ||
                       category.StartsWith($"{LogCategoryCustom}.");
            });

            refreshFilters.Invoke(factory, new[] {filterOptions});


            var startup = new Startup(factory.CreateLogger<Startup>());
            startup.Configure(configBuilder);

            IConfiguration configuration = configBuilder.Build();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton(factory);
            serviceCollection.AddLogging();

            startup.ConfigureServices(serviceCollection);
            
            return serviceCollection.BuildServiceProvider(true);
        }

        
    }
}
