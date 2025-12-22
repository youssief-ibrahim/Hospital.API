using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Hospital.Core.IReposatory;
using Hospital.EF.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Hospital.EF.Reposatory
{
    public class GenericReposatory<T> : IGenericReposatory<T> where T : class
    {
        private readonly ApplicationDbContext context;
        public GenericReposatory(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<T> GetQueryable(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();
            if (query != null)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }
            return query;

        }
        public async Task<List<T>> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = GetQueryable(includes);
            var res=await query.ToListAsync();
            return res;
        }

        public async Task<List<T>> GetAllwithsearch(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes)
        {
            var query = GetQueryable(includes).Where(filter);
            var res = await query.ToListAsync();
            return res;
        }

        public async Task<T> GetById(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = GetQueryable(includes);
            var res = await query.FirstOrDefaultAsync(predicate);
            return res;
        }
        public async Task<T> checkId(int id)
        {
            var res = await context.Set<T>().FindAsync(id);
            return res;
        }

        public async Task<List<T>> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var quary = GetQueryable(includes).Where(predicate);
            return await quary.ToListAsync();
        }

        public async Task Create(T item)
        {
            await context.Set<T>().AddAsync(item);
        }
        public void update(T item)
        {
            context.Set<T>().Update(item);
        }
        public void delete(T item)
        {
            context.Set<T>().Remove(item);
        }

        public void Save()
        {
            context.SaveChanges();
        }


    }
}
