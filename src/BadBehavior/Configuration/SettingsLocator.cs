using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Compilation;

namespace BadBehavior.Configuration
{
    public static class SettingsLocator
    {
        public static SettingsBase FindSettings()
        {
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>();
            var thisAssembly = typeof(SettingsLocator).Assembly;

            var types = from assembly in assemblies
                        where assembly != thisAssembly
                        from type in assembly.GetTypes()
                        where typeof(SettingsBase).IsAssignableFrom(type)
                            && type.GetConstructor(Type.EmptyTypes) != null
                        select type;
            var settings = types.Select(x => Activator.CreateInstance(x))
                .Cast<SettingsBase>().FirstOrDefault();

            return settings ?? new AppConfigSettings();
        }
    }
}
