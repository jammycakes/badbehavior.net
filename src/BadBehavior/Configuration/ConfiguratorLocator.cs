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
        // Fix Github issue 15: ReflectionTypeLoadException.
        // HT: Phil Haack http://haacked.com/archive/2012/07/23/get-all-types-in-an-assembly.aspx

        private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            try {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e) {
                return e.Types.Where(t => t != null);
            }
        }

        public static IConfigurator Find()
        {
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>();
            var thisAssembly = typeof(ConfiguratorLocator).Assembly;

            var types = from assembly in assemblies
                        where assembly != thisAssembly
                        from type in GetLoadableTypes(assembly)
                        where typeof(IConfigurator).IsAssignableFrom(type)
                            && type.GetConstructor(Type.EmptyTypes) != null
                        select type;
            var config = types.Select(x => Activator.CreateInstance(x))
                .Cast<IConfigurator>().FirstOrDefault();

            return config;
        }
    }
}
