
using System;
using System.IO;
using FunctionApp.Injection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, [Inject(typeof(ILoggerFactory))]ILoggerFactory loggerFactory, [Inject(typeof(ClassThatNeedsInjection))]ClassThatNeedsInjection injected)
        {
            try
            {
                injected.DoSomething();
                return new OkObjectResult("");

            }
            catch (Exception ex) when (LogError(loggerFactory, ex))
            {
                throw;
            }
        }

        private static bool LogError(ILoggerFactory factory, Exception exception)
        {
            var logger = factory.CreateLogger(typeof(Function1).FullName);
            logger.LogCritical(exception, exception.Message);

            return true;
        }
    }
}
