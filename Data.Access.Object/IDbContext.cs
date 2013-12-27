using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Object
{
    /// <summary>
    /// Declare an interface to prepare the IoC pattern
    /// </summary>
    public interface IDbContext : IDisposable
    {
        #region Properties contract

        /// <summary>
        /// Return the Target of this proxy
        /// </summary>
        DbContext Target { get; }

        #endregion

        /// <summary>
        /// Returns a DbSet instance for access to entities of the given type in the context, the ObjectStateManager, and the underlying store.
        /// </summary>
        /// <typeparam name="T">The type entity for which a set should be returned.</typeparam>
        /// <returns>A set for the given entity type.</returns>
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        int SaveChanges();

        /// <summary>
        /// Gets a DbEntityEntry<TEntity> entity for the given entity providing access to information about the entity and the ability to perform actions on the entity.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>An entry for the entity.</returns>
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
