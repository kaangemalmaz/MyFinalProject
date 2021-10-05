using Entities.Concrete;
using FluentValidation;
using System;

namespace Business.ValidationRules.FluentValidation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.ProductName).NotEmpty();
            RuleFor(p => p.ProductName).MinimumLength(2);
            RuleFor(p => p.UnitPrice).NotEmpty();
            RuleFor(p => p.UnitPrice).GreaterThan(0);
            //category' i idsi 1 olduğu zaman unit price 10 veya üstü olmak zorundadır.
            //Bu şekilde verilebilir.
            RuleFor(p => p.UnitPrice).GreaterThanOrEqualTo(10).When(p => p.CategoryId == 1);

            //uymak zorunda burada StartwithA olarak özel olarak yazacağız.
            RuleFor(p => p.ProductName).Must(StartWithA).WithMessage("Ürünler A harfi ile başlamalıdır.");

            //eğer with message ile cevap vermezsen o kendisi 19 dilde tarayıcının diline göre mesaj verir.
        }

        private bool StartWithA(string arg) //arg = productname yukarda verdiğin p ile.
        {
            return arg.StartsWith("A"); //burada A ile başlayan bir şey gelir true yoksa false döner.
        }
    }
}
