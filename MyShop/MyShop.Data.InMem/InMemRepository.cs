using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Data.InMem
{
    public class InMemRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> Items;
        string className;

        public InMemRepository()
        {
            className = typeof(T).Name;

            Items = cache[className] as List<T>;

            if (Items == null)
            {
                Items = new List<T>();
            }
        }

        public void Commit()
        {
            cache[className] = Items;
        }

        public T Find(string id)
        {
            T item = Items.Find(i => i.Id == id);

            if (item == null)
            {
                throw new Exception(className + " not found");
            }

            else
            {
                return item;
            }
        }

        public IQueryable<T> Collection()
        {
            return Items.AsQueryable();
        }

        public void Insert(T item)
        {
            Items.Add(item);
        }

        public void Update(T item)
        {
            T itemToUpdate = Items.Find(i => i.Id == item.Id);

            if (itemToUpdate == null)
            {
                throw new Exception(className +" not found");
            }
            else
            {
                itemToUpdate = item;
            }
        }

        public void Delete(string id)
        {
            T itemToDelete = Items.Find(i => i.Id == id);

            if (itemToDelete == null)
            {
                throw new Exception(className + " not found");
            }
            else
            {
                Items.Remove(itemToDelete);
            }
        }
    }
}
