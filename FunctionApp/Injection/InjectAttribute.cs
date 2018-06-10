﻿using System;
using Microsoft.Azure.WebJobs.Description;

namespace FunctionApp.Injection
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        public InjectAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
