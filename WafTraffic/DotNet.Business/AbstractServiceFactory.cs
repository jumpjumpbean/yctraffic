namespace DotNet.Business
{
    using System;
    using System.Reflection;
    using System.Configuration;

    public abstract class AbstractServiceFactory
    {
        private static readonly string serviceFactoryClass = ConfigurationManager.AppSettings["ServiceFactory"];
        private static readonly string servicePath = ConfigurationManager.AppSettings["Service"];

        public IServiceFactory GetServiceFactory()
        {
            return this.GetServiceFactory(servicePath);
        }

        public IServiceFactory GetServiceFactory(string servicePath)
        {
            string typeName = servicePath + "." + serviceFactoryClass;
            return (IServiceFactory) Assembly.Load(servicePath).CreateInstance(typeName);
        }

        public IServiceFactory GetServiceFactory(string servicePath, string serviceFactoryClass)
        {
            string typeName = servicePath + "." + serviceFactoryClass;
            return (IServiceFactory) Assembly.Load(servicePath).CreateInstance(typeName);
        }
    }
}

