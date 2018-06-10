using System;
using System.IO;
using System.Net;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(ILogger<Startup> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ClassThatNeedsInjection>();
        }

        public void Configure(IConfigurationBuilder app)
        {
            var executingAssembly = new FileInfo(Assembly.GetExecutingAssembly().Location);
            _logger.LogInformation($"Using \"{executingAssembly.Directory.FullName}\" as base path to load configuration files.");
            app
                .SetBasePath(executingAssembly.Directory.FullName)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

    }
}
