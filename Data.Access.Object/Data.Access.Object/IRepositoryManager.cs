using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Object
{
    public interface IRepositoryManager : IDisposable
    {
        /// <summary>
        /// Commit changes
        /// </summary>
        void Save();
        /// <summary>
        /// Get the repository for the entity TEntity
        /// </summary>
        /// <typeparam name="TEntity">The entity</typeparam>
        /// <returns>The repository</returns>
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    }
}
