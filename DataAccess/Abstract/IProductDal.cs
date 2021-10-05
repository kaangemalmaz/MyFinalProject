using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IProductDal : IEntityRepository<Product>
    {   //CODE REFACTORING 
        //ürüne özel operasyonları koymak için yapılır.
        List<ProductDetailDto> GetProductDetails();

    }
}
