using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Object
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal IDbContext Context;
        internal IDbSet<TEntity> DbSet;

        public Repository(DbContext dbContext)
        {
            Context = new DbContextProxy(dbContext);
            DbSet = Context.Set<TEntity>();
        }

        public virtual TEntity Find(object id)
        {
            return DbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Attach(entity);
        }

        public virtual void Delete(object id)
        {
            var entity = DbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Attach(entity);
            DbSet.Remove(entity);
        }

        public virtual QueryBuilder<TEntity> Query()
        {
            var repositoryGetFluentHelper =
                new QueryBuilder<TEntity>(this);

            return repositoryGetFluentHelper;
        }

        /// <summary>
        /// This method will be used internally by the QueryBuilder class to query the repository
        /// </summary>
        /// <param name="where">The where clause</param>
        /// <param name="orderBy">Order by clause</param>
        /// <param name="fetchProperties">The properties to fetch</param>
        /// <param name="page">The number of the page to get</param>
        /// <param name="pageSize">The page size to get</param>
        /// <returns></returns>
        internal IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> fetchProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (fetchProperties != null)
                fetchProperties.ForEach(i => { query = query.Include(i); });

            if (where != null)
                query = query.Where(where);

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            return query;
        }
    }
}
