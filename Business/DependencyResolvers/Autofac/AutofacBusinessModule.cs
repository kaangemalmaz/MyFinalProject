using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Http;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //buraya startup kısmındaki yerleri yazıyoruz.
            //bu services add singletona karşılık geliyor.
            //biri senden IProductservice isterse ona productmanager ver demektir aşağıdaki.
            //services.AddSingleton<IProductService, ProductManager>()

            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();
            builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();

            builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>().SingleInstance();



            //Burada eğer autofac kullanacaksan Program.cs de 
            //Host.CreateDefaultBuilder(args)
            //    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            //    .ConfigureContainer<ContainerBuilder>(builder =>
            //    {
            //        builder.RegisterModule(new AutofacBusinessModule());
            //    })
            //    .ConfigureWebHostDefaults(webBuilder

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

            //bu demektir ki eğer sen  bir interface ile bir işlem çalıştıracaksan onun git üzerinde aspect bir metodu var ona bak ve ona göre çalıştır.



        }
    }
}
