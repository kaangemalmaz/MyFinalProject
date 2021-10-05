using Business.Constants;
using Castle.DynamicProxy;
using Core.Extensions;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using System;

using Microsoft.Extensions.DependencyInjection; // _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>(); buradaki get service bundan geldi bilgin olsun 

namespace Business.BusinessAspects.Autofac
{
    // Bu jwt için unutma!!!!
    public class SecuredOperation : MethodInterception
    {
        //Yetkilendirme ile ilgili olarak authorization aspectleri genellikle business a yazılır. Çünkü her projenin yetkilendirme algoritması değişebilir çünkü.

        private string[] _roles; //
        private IHttpContextAccessor _httpContextAccessor; // her istek için bir tread oluşturur. Hepsi için bir httpcontext oluşturur.

        public SecuredOperation(string roles) //bana rolleri ver diyoruz. 
        {
            _roles = roles.Split(','); //roller virgülle ayrılarak gelir bu da split ile virgül ile ayrılarak arraya atılır.
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

            

        }

        protected override void OnBefore(IInvocation invocation)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles(); //metodu çalıştırmadan önce o kullanıcının rollerini bul demek.
            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role))
                {
                    return; // eğer kullanıcının rolleri içinde o varsa return et yani metodu çalıştırmaya devam et.
                }
            }
            throw new Exception(Messages.AuthorizationDenied); //yetkisi yoksa hata ver.
        }
    }
}
