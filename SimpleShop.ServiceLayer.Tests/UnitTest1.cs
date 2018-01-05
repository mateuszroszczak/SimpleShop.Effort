using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity;
using SimpleShop.DataLayer;
using System.Linq;

namespace SimpleShop.ServiceLayer.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void MyTestInitialize()
        {
            EffortProviderFactory.ResetDb();
        }

        private void PrepareData()
        {
            using (var model = new ShopDataContext())
            {
                var category = new Category { Name = "tools" };
                var otherCategory = new Category { Name = "food" };

                category.Products.Add(new Product() 
                { Name = "Notepad", Category = category, Price = 10.0 });
                category.Products.Add(new Product() 
                { Name = "Pencil", Category = category, Price = 4.0 });
                category.Products.Add(new Product() 
                { Name = "Pen", Category = category, Price = 6.0 });
                otherCategory.Products.Add(new Product() 
                { Name = "Pear", Category = otherCategory, Price = 2.0 });

                model.Categories.Add(category);
                model.Categories.Add(otherCategory);

                model.SaveChanges();
            }
        }

        [TestMethod]
        public void Get_ShouldReturnNumberOfProductsForOneCategory()
        {
            PrepareData();

            var productService = new ProductService();
            var result = productService.Get("tools");

            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.All(x => x.CategoryName == "tools"));
        }

        [TestMethod]
        public void Get_ShouldReturnOnlyProductFromCategory()
        {
            PrepareData();

            var productService = new ProductService();
            var result = productService.Get("food");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Pear", result.First().Name);
        }

        [TestMethod]
        public void Get_ShouldReturnSpecialPriceForProductsStartingWithP()
        {
            PrepareData();

            var productService = new ProductService();
            var result = productService.Get("tools");

            Assert.AreEqual(2.0, result.First(x => x.Name == "Pencil").Price);
            Assert.AreEqual(3.0, result.First(x => x.Name == "Pen").Price);
        }

        [TestMethod]
        public void Get_ShouldReturnNormalPriceForOtherProducts()
        {
            PrepareData();

            var productService = new ProductService();
            var result = productService.Get("tools");

            Assert.AreEqual(10.0, result.First(x => x.Name == "Notepad").Price);
        }
    }
}
