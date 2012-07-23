using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Compilation;

namespace BadBehavior.Configuration
{
    public static class ConfiguratorLocator
    {
        public static IConfigurator Find()
        {
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>();
            var thisAssembly = typeof(ConfiguratorLocator).Assembly;

            var types = from assembly in assemblies
                        where assembly != thisAssembly
                        from type in assembly.GetTypes()
                        where typeof(IConfigurator).IsAssignableFrom(type)
                            && type.GetConstructor(Type.EmptyTypes) != null
                        select type;
            var config = types.Select(x => Activator.CreateInstance(x))
                .Cast<IConfigurator>().FirstOrDefault();

            return config;
        }
    }
}
