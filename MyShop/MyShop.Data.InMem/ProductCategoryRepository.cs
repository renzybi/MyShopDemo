using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Data.InMem
{
    public class ProductCategoryRepository
    {
        List<ProductCategory> productCategories;
        ObjectCache cache = MemoryCache.Default;

        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>;

            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }

        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }

        public void Insert(ProductCategory productCategory)
        {
            productCategories.Add(productCategory);
        }

        public void Update(ProductCategory productCategory)
        {
            ProductCategory prodCategoryToBeUpdated = productCategories.Find(p 
                                            => p.Id == productCategory.Id);

            if (prodCategoryToBeUpdated == null)
            {
                throw new Exception("Product Category not found.");
            }
            else
            {
                prodCategoryToBeUpdated.CategoryName = productCategory.CategoryName;
            }
        }

        public ProductCategory Find(string id)
        {
            ProductCategory prodCategory = productCategories.Find(p
                                            => p.Id == id);

            if (prodCategory == null)
            {
                throw new Exception("Product Category not found.");
            }
            else
            {
                return prodCategory;
            }
        }

        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        public void Delete(string id)
        {
            ProductCategory prodCategoryToBeDeleted = productCategories.Find(p
                                            => p.Id == id);

            if (prodCategoryToBeDeleted == null)
            {
                throw new Exception("Product Category not found.");
            }
            else
            {
                productCategories.Remove(prodCategoryToBeDeleted);
            }
        }
    }
}
