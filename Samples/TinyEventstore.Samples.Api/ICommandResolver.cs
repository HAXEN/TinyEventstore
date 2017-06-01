using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TinyEventstore.Samples.Api.Controllers;

namespace TinyEventstore.Samples.Api
{
    public interface ICommandResolver
    {
        IActionResult Execute(string commandName, CommandBase command);
    }

    public class CommandResolver : ICommandResolver
    {
        public IActionResult Execute(string commandName, CommandBase command)
        {
            throw new System.NotImplementedException();
        }
    }

    public static class  CommandHandlerSetup
    {
        public static void UseCommandHandler(this IServiceCollection services)
        {
            services.AddTransient<ICommandResolver, CommandResolver>();
            ResolveHandlers(services, new[] {typeof(CommandBase).GetTypeInfo().Assembly});
        }


        private static void ResolveHandlers(IServiceCollection services, Assembly[] assemblies)
        {
            var found = new List<ICommandHandler>();
            foreach (var assembly in assemblies)
            {
                var handlers = assembly.GetTypes().Where(x => typeof(ICommandHandler).IsAssignableFrom(x)).Cast<ICommandHandler>().ToArray();
                
                found.AddRange(handlers);
            }
            services.AddTransient<IEnumerable<ICommandHandler>>();
        }
    }
}
