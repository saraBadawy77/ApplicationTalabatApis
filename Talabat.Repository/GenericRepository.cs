using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {

        private readonly StoreContext dbContext;
        public GenericRepository(StoreContext _dbContext)
        {
            dbContext = _dbContext; 
        }

       

        public async Task<IReadOnlyList<T>> GetAllAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIdAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
        {

            return await ApplySpecification(spec).CountAsync();
        }

      

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(dbContext.Set<T>().AsQueryable(), spec);
        }



        public async Task AddAsync(T entity)
        => await dbContext.Set<T>().AddAsync(entity);

        public void Update(T entity)
        => dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
        => dbContext.Set<T>().Remove(entity);


    }
}
