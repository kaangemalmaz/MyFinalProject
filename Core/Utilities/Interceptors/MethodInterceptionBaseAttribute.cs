using Castle.DynamicProxy;
using System;

namespace Core.Utilities.Interceptors
{
    //Interceptors demek araya girmek demek ne zaman loglama yapılacak ne zaman cache
    //öncesinde sonrasında hata sırasında başarılı bitmesi durumunda çalıştır gibi.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)] // burada hem databasee hemde txt ye yaz anlamında allowmultiple = true yapılır.
    public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor
    {
        public int Priority { get; set; } // hangi özellik önce çalışsın onu gösterir.

        public virtual void Intercept(IInvocation invocation)
        {

        }
    }
}
