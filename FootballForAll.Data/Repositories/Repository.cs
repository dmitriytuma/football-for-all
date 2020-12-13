using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FootballForAll.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly ApplicationDbContext DbContext;
        private readonly DbSet<TEntity> DbSet;

        public Repository(ApplicationDbContext applcationDbContext)
        {
            DbContext = applcationDbContext;
            DbSet = DbContext.Set<TEntity>();
        }

        public Task AddAsync(TEntity entity)
        {
            return DbSet.AddAsync(entity).AsTask();
        }

        public IQueryable<TEntity> All()
        {
            return DbSet;
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public TEntity Get(int id)
        {
            return DbSet.Find(id);
        }

        public Task<int> SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            var entry = DbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbContext?.Dispose();
            }
        }

        #endregion
    }
}
