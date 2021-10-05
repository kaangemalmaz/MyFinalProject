using Castle.DynamicProxy;
using Core.CrossCuttingConcers.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheAspect : MethodInterception
    {
        private int _duration;
        private ICacheManager _cacheManager;

        public CacheAspect(int duration = 60)
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        public override void Intercept(IInvocation invocation)
        {
            //invocation.Method.ReflectedType.FullName => namespace + class ismidir ama base class ismidir.
            //Business.Abstract.IProductService service demek
            //invocation.Method.Name => metodun ismidir. GetAll gibi.
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            
            // metodun parametrelerini liste haline getir ve al demek.
            var arguments = invocation.Arguments.ToList();

            //parametreleri al , ile ayırarak tek bir key oluştur demek 
            ////Business.Concrete.IProductService.Get(1) gibi bir key oluşturur. Bu unique olsun diye verilir.
            ///// null dönebilir diye ?? yoksa null geç demektir.
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";

            //burada ilk olarak cachede key varmı diye bakılır.
            if (_cacheManager.IsAdd(key))
            {
                invocation.ReturnValue = _cacheManager.Get(key); // bu veritabanına gitmesinde cachemanagerdaki cachedeki tutulan değeri getirsin demektir.
                return;
                //eğer key varsa olduğunu dön demektir. Yani cache de yapı var demektir bu.
            }

            //yoksa yapıyı çalıştır demektir.
            invocation.Proceed();

            //aynı zaman da cache de yapıyı ekle demek.
            _cacheManager.Add(key, invocation.ReturnValue, _duration);
        }
    }
}
