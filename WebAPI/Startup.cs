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
            //Buradaki yap� Autofac, Ninject, CastleWindsor, StructureMap, LightInject, DryInjecy --> IoC Container
            //Yukardaki bu yap�lar a�a��daki i�leri yani ioc container altyap�s�n� sunuyor.
            //Biz projede sadece injection yapm�yoruz. O y�zden yukardaki yap�lar� kullanmaya ihtiya� duyuyoruz o y�zden de yukardaki yap�lar� kulland���m�z i�in injection� da burada vererek buradaki yap�lar� temiz tutmay� sa�l�yoruz.
            //Ama biz projemizde AoP yap�s� kuraca��z => mesela t�m projeyi loglayaca��z bunun i�in yukardaki yap�lar bize laz�m demektir.
            //AOP => bir metotun �n�nde , metot s�ras�nda, sonras�nda senin kodlamana g�re �al��an yap�d�r. Bunu sa�lamak i�in yukardaki yap�lar� kullan�r�z. Validate, Log, Cache, RemoveCache, Transaction bir yap� uygula, Performance �l��m� sa�lar. Autofac bize AOP imkan� sunuyor.



            //services.AddSingleton<IProductService, ProductManager>(); // Biri constructorda IProductService isterse arkada ona ProductManager ver demektir bu.
            //services.AddSingleton<IProductDal, EfProductDal>(); // => static gibi d���n. Bana arka planda bir referans olu�tur. E�er birisi senden IProductDal istersen EfProductDal ver ona demek bu olu�tur ver. Singleton t�m bellekte bir tane instance olu�turur. ��inde data tutmad���n her �eyi singleton yap. E�er bir sepet tutmuyorsan mesela burada sepet kullan�yorsan biri bir ekleme yapt���nda herkesde ekleme i�lemi yapar. O y�zden data tutmayan bir yap� olmas� �ok �nemli.



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

            app.ConfigureCustomExceptionMiddleware(); // exception middleware i�in unutma

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
