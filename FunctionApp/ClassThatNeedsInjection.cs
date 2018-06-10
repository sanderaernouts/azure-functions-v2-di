using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class ClassThatNeedsInjection
    {
        private readonly ILogger<ClassThatNeedsInjection> _logger;

        public ClassThatNeedsInjection(ILogger<ClassThatNeedsInjection> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void DoSomething()
        {
            _logger.LogInformation("Doing something");
        }
    }
}
