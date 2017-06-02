using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace TinyEventstore.Commanding
{
    public class CommandResolver : ICommandResolver
    {
        public IActionResult Execute(string commandName, CommandBase command)
        {
            throw new System.NotImplementedException();
        }

        public static IEnumerable<Type> KnownHandlers(params Assembly[] assemblies)
        {
            var foundTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                foundTypes.AddRange(assembly.ExportedTypes.Where(x => typeof(ICommandHandler).GetTypeInfo().IsAssignableFrom(x.GetTypeInfo()) && !x.GetTypeInfo().IsAbstract).ToArray());
            }
            return foundTypes;
        }
    }
}