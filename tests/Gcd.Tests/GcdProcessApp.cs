﻿using Gcd.CommandHandlers;
using Gcd.Extensions;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace Gcd.Tests
{
    public class GcdProcessApp : IGcdProcess
    {
        IConsole _console;
        CommandLineApplication _app; 
        public GcdProcessApp()
        {
            _console = new FakeConsole();
            var assembly = typeof(Program).Assembly;
            var services = new ServiceCollection()
                .AddSingleton<IVersionizeCommandHandler, VersionizeCommandHandler>()
                .AddScoped<ILabViewProjectProvider, LabViewProjectProvider>()
                .AddSingleton<IConsole>(_console)
                .AddMediatR(config =>
                {
                    config.RegisterServicesFromAssembly(assembly);
                });

            var serviceProvider = services.BuildServiceProvider();

            _app = new CommandLineApplication<Program>()
            {
                Name = "gcd",
                Description = "CI/CD tool for G programmers with OCDddd",
            };

            _app.UseGcdCmd(serviceProvider);
        }

        public GcdProcessResponse Run(string[] request)
        {
            return Run(new GcdProcessRequest() { Arguments = request });
        }
        public GcdProcessResponse Run(GcdProcessRequest request)
        {
            var result = _app.Execute(request.Arguments);

            return new GcdProcessResponse
            {
                Error = _console.Error.ToString(),
                Out = _console.Out.ToString(),
                Return = result
            };
        }
    }
}