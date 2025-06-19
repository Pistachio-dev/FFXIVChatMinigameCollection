using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersistentModel.Repository.Generic
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly MinigameCollectionDbContext _ctx;
      
        public Repository(MinigameCollectionDbContext minigameCollectionDbContext)
        {
            _ctx = minigameCollectionDbContext;
            _ctx.ApplyPendingMigrations();
        }

        public void Add(TEntity entity)
        {
            _ctx.Set<TEntity>().Add(entity);
            _ctx.SaveChanges();
        }
        public void AddMany(IEnumerable<TEntity> entities)
        {
            _ctx.Set<TEntity>().AddRange(entities);
            _ctx.SaveChanges();
        }
        public void Delete(TEntity entity)
        {
            _ctx.Set<TEntity>().Remove(entity);
            _ctx.SaveChanges();
        }
        public void DeleteMany(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = Find(predicate);
            _ctx.Set<TEntity>().RemoveRange(entities);
            _ctx.SaveChanges();
        }
        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null)
        {
            return Get(findOptions).FirstOrDefault(predicate)!;
        }
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null)
        {
            return Get(findOptions).Where(predicate);
        }
        public IQueryable<TEntity> GetAll(FindOptions? findOptions = null)
        {
            return Get(findOptions);
        }
        public void Update(TEntity entity)
        {
            _ctx.Set<TEntity>().Update(entity);
            _ctx.SaveChanges();
        }
        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _ctx.Set<TEntity>().Any(predicate);
        }
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _ctx.Set<TEntity>().Count(predicate);
        }
        private DbSet<TEntity> Get(FindOptions? findOptions = null)
        {
            findOptions ??= new FindOptions();
            var entity = _ctx.Set<TEntity>();
            if (findOptions.IsAsNoTracking && findOptions.IsIgnoreAutoIncludes)
            {
                entity.IgnoreAutoIncludes().AsNoTracking();
            }
            else if (findOptions.IsIgnoreAutoIncludes)
            {
                entity.IgnoreAutoIncludes();
            }
            else if (findOptions.IsAsNoTracking)
            {
                entity.AsNoTracking();
            }
            return entity;
        }
    }
}
