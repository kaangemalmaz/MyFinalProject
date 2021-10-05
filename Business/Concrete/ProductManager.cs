using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcers.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {

        // Bir entity manager kendisi dışında bir dalı enjecte edemez unutma!!!!
        // yani buraya categoryDal eklenemez.... unutma
        //ama categoryService 'i entejecte edebilirsin.
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        //Claim => product.add veya admin yetkisine sahip olması gerekiyor demektir Product eklemek için...
        //Encryption =>  Buradaki data çözülebiliyor. yine gizliyorsun ama bunu çözebiliyorsun unutma.
        //Hashing => Bir datayı karşı taraf okuyamasın diye yapılan şeylerdir. Burada passwordu hashleriz. Sallamasyon bir data oluşuyor. Buradaki data çözülemiyor dikkat et.
        //md5, SHA1 => buradaki hashleme algoritmalarıdır. bununla hashlenir. Böylece veri gizli kalır.
        //Salting => tuzlama kullanıcının girdiği şifreye mesale daha iyi hale getirmek için bazı eklemeler yapmak demektir.
        //[SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))] //Add metonunu productValidatoru kullanarak doğrulama işlemi yap demek.
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            //business codes => iş gereksinimlerine uygunluktur. 
            //validation => şifre min. karakter olmalı, şuna uymalı gibi kararlar, doğrulama denir buna. Bunlar girilen verinin veri bütünlüğü ile ilgilidir. Bunları başka yerde de kullanabileceklerin burada olmalı ancak bu projeye özel olanlar burada business kod olarak yazılmalıdır.

            //ValidationTool.Validate(new ProductValidator(), product);

            //if (CheckIfProductCountOfCategoryCorrect(product.CategoryId).Success)
            //{
            //    if (CheckDifferentNameForProduct(product.ProductName).Success)
            //    {
            //        _productDal.Add(product);
            //        return new SuccessResult(Messages.ProductAdded);
            //    }
            //}
            //return new ErrorResult();


            //Burada hata döndüğü zaman direk onu döndürecek bir yapı kurduk zaten eğer burası null geliyorsa hata yok demektir unutma.
            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                                               CheckDifferentNameForProduct(product.ProductName),
                                               CheckCategoryCount());

            //hata döndü demektir aşağısı o durumda direk IResult dönüyor zaten direk yolla gitsin metodun dönüş isteğide aynı.
            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);




        }

        [CacheAspect] 
        //bir kez çağırıldıysa bidaha bidaha veri tabanına gitmemesi için yapılabilir.
        //istediğin kadar süre verebilirsin unutma.
        //ürün eklendiğinde / silindiğinde / güncellendiğinde cache silinmelidir.
        //key value yapısı ile tutulur.
        public IDataResult<List<Product>> GetAll()
        {
            //iş kodları if böyleyse iş söyleyse
            // yetkisi var mı ? yetkisi falan varsa tüm kuralları geçtiyse aşağıdaki dataaccess kısmına gitmesi gerekmektedir.
            //business da inmemory veya dbcontext olamaz.

            //if (DateTime.Now.Hour != 15)
            //{
            //    return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            //}
            //Thread.Sleep(5000);
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id), Messages.ProductListedByCategory);
        }


        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(i => i.ProductId == productId), Messages.GetProductById);
        }

        public IDataResult<List<Product>> GetMinAndMaxLimitProducts(int min, int max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max), Messages.MinMaxProductList);
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            if (DateTime.Now.Hour == 00)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails(), Messages.GetProductDetail);
        }


        //Bu bir iş kuralı ve bunu birden fazla metotda kullanma olasılığın var o yüzden bu kurulu ayrıyetten yazılır ve verilen isim kuralın anlaşılması için çok önemlidir dikkat et!
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 15)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckDifferentNameForProduct(string productName)
        {
            //var result = _productDal.GetAll(i => i.ProductName.ToLower() == productName.ToLower());
            //if (result == null)


            //Any şuna uyan kayıt var mı demektir unutma. any sadece bool döndürür.
            var result = _productDal.GetAll(i => i.ProductName.ToLower() == productName.ToLower()).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }


        private IResult CheckCategoryCount()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CheckCategoryCount);
            }
            return new SuccessResult();
        }

        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult();
        }


        //[TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
