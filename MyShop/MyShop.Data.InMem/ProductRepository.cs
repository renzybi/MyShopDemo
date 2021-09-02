using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Data.InMem
{
    public class ProductRepository
    {
        MemoryCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository()
        {
            products = cache["products"] as List<Product>;
            if (products == null)
            {
                products = new List<Product>();
            }
        }

        public void Commit()
        {
            cache["products"] = products;
        }

        public void Insert(Product product)
        {
            products.Add(product);
        }

        public void Update(Product product)
        {
            Product productToBeUpdated = products.Find(p => p.Id == product.Id);

            if (productToBeUpdated != null)
            {
                productToBeUpdated.Name = product.Name;
                productToBeUpdated.Description = product.Description;
                productToBeUpdated.Category = product.Category;
                productToBeUpdated.Image = product.Image;
                productToBeUpdated.Price = product.Price;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        public Product Find(string id)
        {
            Product product = products.Find(p => p.Id == id);

            if (product == null)
            {
                throw new Exception("Product not Found");
            }

            return product;
        }

        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }

        public void Delete(string id)
        {
            Product productToBeDelete = products.Find(p => p.Id == id);

            if (productToBeDelete == null)
            {
                throw new Exception("Product not found");
            }

            products.Remove(productToBeDelete);
        }
    }
}
