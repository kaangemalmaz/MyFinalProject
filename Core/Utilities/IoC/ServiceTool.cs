using Microsoft.Extensions.DependencyInjection;
using System;

namespace Core.Utilities.IoC
{
    public static class ServiceTool
    {

        //IoC => inversion of control

        //_productService = ServiceTool.ServiceProvider.GetService<IProductService>(); 
        //bunun sayesinde yukarda yazılan ile autofac de injection değerlerini almanı sağlar yani productservisi çağırabilmene olanak tanır.
        public static IServiceProvider ServiceProvider { get; private set; }

        public static IServiceCollection Create(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}
