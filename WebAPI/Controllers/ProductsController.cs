using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")] //bu insanlar bize nasıl ulaşacak.
    [ApiController] //bunlar attribute
    public class ProductsController : ControllerBase
    {
        //Loosely coupled => interface bağlı olduğu için direk olarak serbest bağımlılık
        // _ ile yazılır naming convention denir buna.
        // biz burada product manager vr diyoruz ama constructar üzerinden verilen şeyler getter olduğu için direk olarak productService ulaşamıyoruz. bunun yerine _productservice field vererek buna ulaşmasını sağlarız.
        //Ama elimizde hala daha smut bir manager yok bu durumda işe IoC giriyor => Inversion of Control
        //IoC bir kutu => burada referanslar koyuoruz yani her şeyi newliyoruz buradan kullanıyoruz direk.
        // bu sadece bir configurasyon arkada direk instance oluşturan yapıdır aslında. interfacede o instance için referans numarası tutturulur.

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("getall")] // => buna alias denir. Bu durumda metotların isilerini de düzeltebilirsin.
        public IActionResult GetAll()
        {
            //IProductService _productService = new ProductManager(new EfProductDal());
            var result = _productService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result); // bad requestte düşmüyor bir dikkat et buna 
        }

        //[HttpGet("/{id}")]
        //[HttpGet("getbyid")] => buna alias denir. Bu durumda metotların isilerini de düzeltebilirsin.
        //postmande https://localhost:44389/api/products/getbyid?id=1 şeklnde kullanılır.
        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            //IProductService _productService = new ProductManager(new EfProductDal());
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result); // bad requestte düşmüyor bir dikkat et buna 
        }


        [HttpGet("getbycategory")]
        public IActionResult GetByCategory(int categoryId)
        {
            //IProductService _productService = new ProductManager(new EfProductDal());
            var result = _productService.GetAllByCategoryId(categoryId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result); // bad requestte düşmüyor bir dikkat et buna 
        }


        [HttpGet("getProductDetails")]
        public IActionResult GetProductDetails()
        {
            //IProductService _productService = new ProductManager(new EfProductDal());
            var result = _productService.GetProductDetails();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result); // bad requestte düşmüyor bir dikkat et buna 
        }


        [HttpPost("add")] // => buna alias denir. Bu durumda metotların isilerini de düzeltebilirsin.
        public IActionResult Add(Product product)
        {
            var result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
