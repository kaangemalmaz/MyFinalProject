using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            //DTO = Data Transfer Object
            ProductTest();
            //IoC
            //CategoryTest();
        }

        private static void CategoryTest()
        {
            CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
            foreach (var category in categoryManager.GetAll().Data)
            {
                Console.WriteLine(category.CategoryName);
            }
        }

        private static void ProductTest()
        {
            ProductManager productManager = new ProductManager(productDal: new EfProductDal(), new CategoryManager(new EfCategoryDal()));

            var result = productManager.GetProductDetails();

            if (result.Success)
            {
                foreach (var p in productManager.GetProductDetails().Data)
                {
                    Console.WriteLine(p.ProductName + "/" + p.CategoryName);
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }



        }
    }
}
