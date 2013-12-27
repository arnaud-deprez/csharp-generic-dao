using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Object
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Find an entity by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Find(object id);
        /// <summary>
        /// Insert a new entity with its graph (linked entities).
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);
        /// <summary>
        /// Update an entity or insert it without its graph
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);
        /// <summary>
        /// Delete an entity by its id
        /// </summary>
        /// <param name="id"></param>
        void Delete(object id);
        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);
        /// <summary>
        /// Create a QueryBuilder
        /// </summary>
        /// <returns></returns>
        QueryBuilder<TEntity> Query();
    }
}
