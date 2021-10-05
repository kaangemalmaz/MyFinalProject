using Castle.DynamicProxy;
using Core.CrossCuttingConcers.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType) // bana neyi validate edeceğimin type i ver demek
            //attribute de type ile gitmek zorunda olduğunu unutma.
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
                //bu validator gelecek demektir. Eğer validator gelmezse hata ver anlamına gelir buradaki yapı.
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değildir.");
            }

            _validatorType = validatorType;
        }

        
        protected override void OnBefore(IInvocation invocation)  //invocation metod demek unutma.
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);  //reflection => çalışma anında bir şeyleri çalıştırabilmenizi sağlıyor. birşeyleri newlemeyi çalışma anında yaptırabilir.
            var entityType = _validatorType.BaseType.GetGenericArguments()[0]; // productvalidatorın çalışma tipini bul diyor.
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType); //mesela add metodunun parametrelerine bak. onun parametrelerini bul demek. İlgili metodun parametreleri demek orada entityType tipine denk gelen base sınıftaki genericten aldığımız parametrelerde eşleşenleri bul demek.
            foreach (var entity in entities) //herbirini tek tek gez
            {
                ValidationTool.Validate(validator, entity); //bununla validate et.
            }
        }
    }
}
