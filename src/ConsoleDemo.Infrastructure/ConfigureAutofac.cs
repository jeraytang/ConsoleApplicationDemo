using System;
using System.Reflection;
using System.Runtime.Loader;
using Autofac;

namespace ConsoleDemo.Infrastructure
{
    public class ConfigureAutofac : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = GetAssembly("ConsoleDemo.Service");
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        private static Assembly GetAssembly(string assemblyName)
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(AppContext.BaseDirectory + $"{assemblyName}.dll");
            return assembly;
        }
    }
}