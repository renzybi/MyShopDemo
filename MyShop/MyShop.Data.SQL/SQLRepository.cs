using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Data.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal AppDbContext _db;
        internal DbSet<T> dbSet;

        public SQLRepository(AppDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }
        public IQueryable<T> Collection()
        {
            return dbSet;
        }

        public void Commit()
        {
            _db.SaveChanges();
        }

        public void Delete(string id)
        {
            var item = dbSet.Find(id);

            if (_db.Entry(item).State == EntityState.Detached)
            {
                dbSet.Attach(item);
            }

            dbSet.Remove(item);
        }

        public T Find(string id)
        {
            return dbSet.Find(id);
        }

        public void Insert(T item)
        {
            dbSet.Add(item);
        }

        public void Update(T item)
        {
            dbSet.Attach(item);
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
