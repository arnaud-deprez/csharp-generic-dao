using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Object
{
    /// <summary>
    /// Helper class to build a query.
    /// You can define a where clause, an order by clause and fetch some properties in one time
    /// </summary>
    /// <typeparam name="TEntity">The entity type on which the query will be perform</typeparam>
    public sealed class QueryBuilder<TEntity> where TEntity : class
    {
        #region Private Variables

        private readonly Repository<TEntity> _repository;
        private readonly List<Expression<Func<TEntity, object>>> _fetchProperties;
        private Expression<Func<TEntity, bool>> _whereClause;
        private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> _orderByQuerable;
        private int? _page;
        private int? _pageSize;

        #endregion

        internal QueryBuilder(Repository<TEntity> repository)
        {
            _repository = repository;
            _fetchProperties = new List<Expression<Func<TEntity, object>>>();
        }

        /// <summary>
        /// Define a where clause
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public QueryBuilder<TEntity> Where(Expression<Func<TEntity, bool>> where)
        {
            _whereClause = where;
            return this;
        }

        /// <summary>
        /// Define an order by clause
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public QueryBuilder<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            _orderByQuerable = orderBy;
            return this;
        }

        /// <summary>
        /// Add properties to fetch
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public QueryBuilder<TEntity> Fetch(Expression<Func<TEntity, object>> expression)
        {
            _fetchProperties.Add(expression);
            return this;
        }

        /// <summary>
        /// Get the result in an IEnumerable object
        /// </summary>
        /// <param name="page">The number of the page</param>
        /// <param name="pageSize">The size of the page</param>
        /// <returns></returns>
        public IEnumerable<TEntity> Get(int? page = null, int? pageSize = null)
        {
            _page = page;
            _pageSize = pageSize;

            return _repository.Get(_whereClause, _orderByQuerable, _fetchProperties, _page, _pageSize);
        }

        /// <summary>
        /// Count the number of returned records regarding the where clause
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return _repository.Get(_whereClause).Count();
        }
    }
}
