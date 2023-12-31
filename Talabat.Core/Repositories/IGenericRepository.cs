using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T :BaseEntity
    {

        Task<IReadOnlyList<T>> GetAllAsync(ISpecification<T> spec);

        Task<T> GetByIdAsync(ISpecification<T> spec);

        Task<int> GetCountAsync(ISpecification<T> spec);
       
        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

    }
}
