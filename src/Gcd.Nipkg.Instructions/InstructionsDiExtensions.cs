using CSharpFunctionalExtensions;
using Gcd.Tests.UnitTest;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services.DI
{
    public static class InstructionsDiExtensions
    {

        public static IServiceCollection RegisterInstructions(this IServiceCollection services)
        {
            services.AddScoped<IInstructionsSerializer, InstructionsSerializer>();
            return services;
        }
    }
}
