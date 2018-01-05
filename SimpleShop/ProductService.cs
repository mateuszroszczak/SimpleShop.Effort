using SimpleShop.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleShop
{
    public class ProductService
    {
        public List<ProductModel> Get(string category)
        {
            using (var context = new ShopDataContext())
            {
                return context.Products
                    .Where(x => x.Category.Name == category)
                    .Select(x => 
                        new ProductModel 
                        {  
                            Id = x.Id,
                            CategoryName = x.Category.Name,
                            Price = x.Name.StartsWith("P") ? x.Price * 0.5 : x.Price,
                            Name = x.Name
                        })
                    .ToList();
            }
        }
    }
}
