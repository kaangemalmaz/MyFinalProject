using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Concrete.InMemory
{
    public class InMemoryProductDal : IProductDal
    {
        List<Product> _products; //global değişkenler alt çizgi ile verilir.

        public InMemoryProductDal()
        {
            _products = new List<Product>
            {
                new Product{ProductId=1,CategoryId = 1, ProductName="Bardak", UnitPrice = 15, UnitsInStock=15},
                new Product{ProductId=1,CategoryId = 2, ProductName="Kamera", UnitPrice = 500, UnitsInStock=3},
                new Product{ProductId=1,CategoryId = 2, ProductName="Telefon", UnitPrice = 1500, UnitsInStock=2},
                new Product{ProductId=1,CategoryId = 2, ProductName="Klavye", UnitPrice = 150, UnitsInStock=65},
                new Product{ProductId=1,CategoryId = 2, ProductName="Fare", UnitPrice = 85, UnitsInStock=1}
            };
        }

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public void Delete(Product product)
        {
            Product producttoDelete;

            //foreach (var p in _products)
            //{
            //    if (product.ProductId == p.ProductId)
            //    {
            //        producttoDelete = p;
            //    }
            //}
            //LINQ - Language Integrated Query. Linq liste olan tüm dataları yukardaki gibi dönmek yerine tek bir satır ile dönmeni sağlayan yapıdır.

            producttoDelete = _products.SingleOrDefault(i => i.ProductId == product.ProductId);

            _products.Remove(producttoDelete);
        }

        public Product Get(Expression<Func<Product, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            return _products;
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllByCategory(int categoryId)
        {
            return _products.Where(i => i.CategoryId == categoryId).ToList();
        }

        public List<Product> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ProductDetailDto> GetProductDetails()
        {
            throw new NotImplementedException();
        }

        public void Update(Product product)
        {
            var producttoUpdate = _products.SingleOrDefault(i => i.ProductId == product.ProductId);
            producttoUpdate.ProductName = product.ProductName;
            producttoUpdate.UnitPrice = product.UnitPrice;
            producttoUpdate.CategoryId = product.CategoryId;
            producttoUpdate.UnitsInStock = product.UnitsInStock;

        }
    }
}
