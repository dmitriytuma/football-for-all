using System;
using System.Linq;
using System.Threading.Tasks;

namespace FootballForAll.Data.Repositories
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        TEntity Get(int id);

        IQueryable<TEntity> All();

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<int> SaveChangesAsync();
    }
}
