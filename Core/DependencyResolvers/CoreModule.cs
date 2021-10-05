using Core.CrossCuttingConcers.Caching;
using Core.CrossCuttingConcers.Caching.Microsoft;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        //uygulama seviyesinde bağımlıkları çözeceğimiz yerdir.
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache(); //memory cachin çalışması için bu satır eklenir. .net core kendisi otomatik injection yapar burada.
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();
        }
    }
}
