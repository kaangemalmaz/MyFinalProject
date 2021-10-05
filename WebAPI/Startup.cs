using Business.Abstract;
using Business.Concrete;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddCors();

            // BURADA ONCESINDE AUTOFAC ILE EKLENEN LIFETIME SURELERI VERILMISTI.
            //Buradaki yapý Autofac, Ninject, CastleWindsor, StructureMap, LightInject, DryInjecy --> IoC Container
            //Yukardaki bu yapýlar aþaðýdaki iþleri yani ioc container altyapýsýný sunuyor.
            //Biz projede sadece injection yapmýyoruz. O yüzden yukardaki yapýlarý kullanmaya ihtiyaç duyuyoruz o yüzden de yukardaki yapýlarý kullandýðýmýz için injectioný da burada vererek buradaki yapýlarý temiz tutmayý saðlýyoruz.
            //Ama biz projemizde AoP yapýsý kuracaðýz => mesela tüm projeyi loglayacaðýz bunun için yukardaki yapýlar bize lazým demektir.
            //AOP => bir metotun önünde , metot sýrasýnda, sonrasýnda senin kodlamana göre çalýþan yapýdýr. Bunu saðlamak için yukardaki yapýlarý kullanýrýz. Validate, Log, Cache, RemoveCache, Transaction bir yapý uygula, Performance ölçümü saðlar. Autofac bize AOP imkaný sunuyor.



            //services.AddSingleton<IProductService, ProductManager>(); // Biri constructorda IProductService isterse arkada ona ProductManager ver demektir bu.
            //services.AddSingleton<IProductDal, EfProductDal>(); // => static gibi düþün. Bana arka planda bir referans oluþtur. Eðer birisi senden IProductDal istersen EfProductDal ver ona demek bu oluþtur ver. Singleton tüm bellekte bir tane instance oluþturur. Ýçinde data tutmadýðýn her þeyi singleton yap. Eðer bir sepet tutmuyorsan mesela burada sepet kullanýyorsan biri bir ekleme yaptýðýnda herkesde ekleme iþlemi yapar. O yüzden data tutmayan bir yapý olmasý çok önemli.



            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };
                });

            services.AddDependencyResolvers(new ICoreModule[]
            {
                new CoreModule()
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            app.ConfigureCustomExceptionMiddleware(); // exception middleware için unutma

            app.UseCors(builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
