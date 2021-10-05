using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;

namespace Core.Utilities.Interceptors
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute> //reflection
                (true).ToList(); //buradaki classın üzerindeki attributeler toplanır. burada classın  classa özelde bir methodu olabilir unutma.
            var methodAttributes = type.GetMethod(method.Name) //reflection meth
                .GetCustomAttributes<MethodInterceptionBaseAttribute>(true); //method atrributeleri alınır. validation, logger, cache
            classAttributes.AddRange(methodAttributes); //hepsini bir arraye at
            //classAttributes.Add(new ExceptionLogAspect(typeof(FileLogger))); => burası oluşan tüm metodlara ekleme yapmayı sağlar yani sonradan stajyer geldi metot yazdı ama loglamayı unutmuş gibi bir problem çekmezsin çok önemli özellik unutma.

            return classAttributes.OrderBy(x => x.Priority).ToArray(); //arrayi öcelik sırasına göre diz yolla. yani metodun classın üzerinde ilk ne çalışıcak ona karar vererek yolla.
        }
    }
}
