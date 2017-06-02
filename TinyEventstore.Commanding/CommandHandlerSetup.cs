using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TinyEventstore.Commanding
{
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
                var handlers = assembly.ExportedTypes.Where(x => typeof(ICommandHandler).GetTypeInfo().IsAssignableFrom(x.GetTypeInfo())).Cast<ICommandHandler>().ToArray();
                
                found.AddRange(handlers);
            }
            services.AddTransient<IEnumerable<ICommandHandler>>();
        }
    }
}
