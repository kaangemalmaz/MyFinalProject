using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IProductService
    {
        //burada liste olarak yollayarak T ye liste yollayabilirsin.
        IDataResult<List<Product>> GetAll();

        IDataResult<List<Product>> GetAllByCategoryId(int id);

        IDataResult<List<Product>> GetMinAndMaxLimitProducts(int min, int max);

        IDataResult<List<ProductDetailDto>> GetProductDetails();

        IDataResult<Product> GetById(int productId);

        IResult Add(Product product);

        IResult Update(Product product);

        IResult AddTransactionalTest(Product product);
    }
}
