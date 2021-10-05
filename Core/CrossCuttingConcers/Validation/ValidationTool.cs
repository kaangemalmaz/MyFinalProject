using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcers.Validation
{
    // CrossCuttingConcers tüm katmanlara özel yazılabilecek kodları içeren yapıdır.
    // Loglama, Cache, Transaction gibi yapılardır.
    // Bu yapılar static yazılır bir kere instance alsa yeterli olur zaten hep aynı işi yapmaktadır.
    public static class ValidationTool
    {
        public static void Validate(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity); //burada Context Product generic nesnesi için çalışacak ve parantez içindeki product metodun içinden gelmektedir.
            var result = validator.Validate(context);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }


        }



        //var context = new ValidationContext<Product>(product); //burada Context Product generic nesnesi için çalışacak ve parantez içindeki product metodun içinden gelmektedir.
        //ProductValidator productValidator = new ProductValidator();
        //var result = productValidator.Validate(context);
        //    if (!result.IsValid)
        //    {
        //        throw new ValidationException(result.Errors);
        //}
    }
}
