using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockContext<T> : IRepository<T> where T : BaseEntity
    {
        List<T> items;
        string className;

        public MockContext()
        {
            items = new List<T>();
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Commit()
        {
            return;
        }

        public void Delete(string id)
        {
            T itemToBeDeleted = items.Find(i => i.Id == id);

            if (itemToBeDeleted != null)
            {
                items.Remove(itemToBeDeleted);
            }
            else
            {
                throw new Exception(className + "Not found.");
            }
        }

        public T Find(string id)
        {
            T item =  items.Find(i => i.Id == id);

            if (item != null)
            {
                return item;
            }

            else
            {
                throw new Exception(className + "Not found.");
            }
        }

        public void Insert(T item)
        {
            items.Add(item);
        }

        public void Update(T item)
        {
            T itemToBeUpdated = items.Find(i => i.Id == item.Id);

            if (itemToBeUpdated != null)
            {
                itemToBeUpdated = item;
            }
            else
            {
                throw new Exception(className + "Not found.");
            }
        }
    }
}
