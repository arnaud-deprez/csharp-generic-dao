using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Object
{
    public partial class DbContextProxy : IDbContext
    {
        /// <summary>
        /// Unique identifier per instance
        /// </summary>
        private readonly Guid _instanceId;
        /// <summary>
        /// DbSet container
        /// </summary>
        private IDictionary _dbSetContainer;
        /// <summary>
        /// The actual Db context
        /// </summary>
        private DbContext _dbContext;

        public DbContextProxy(DbContext context)
            : base()
        {
            _dbContext = context;
            _instanceId = Guid.NewGuid();
            Init();
        }

        public Guid InstanceId
        {
            get { return _instanceId; }
        }

        public DbContext Target
        {
            get { return _dbContext; }
        }

        #region DbSetContainer initialization

        protected virtual void Init()
        {
            if (_dbSetContainer == null)
                _dbSetContainer = new Hashtable();

            Type t = _dbContext.GetType();

            foreach (PropertyInfo p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!p.PropertyType.IsGenericType)
                    continue;

                Type genericType = p.PropertyType.GetGenericTypeDefinition();
                if (genericType != typeof(IDbSet<>) && genericType != typeof(DbSet<>))
                    continue;

                if (!p.CanRead)
                    continue;

                _dbSetContainer.Add(p.PropertyType.GenericTypeArguments[0], p.GetValue(_dbContext));
            }
        }

        #endregion

        #region Overriden Properties and Methods

        public IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            if (_dbSetContainer == null)
                Init();

            return _dbSetContainer.Contains(typeof(TEntity)) ? _dbSetContainer[typeof(TEntity)] as IDbSet<TEntity> : _dbContext.Set<TEntity>();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return _dbContext.Entry<TEntity>(entity);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        #endregion
    }
}
