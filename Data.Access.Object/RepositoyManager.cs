using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Object
{
    public partial class RepositoyManager : IRepositoryManager
    {
        private readonly IDbContext _context;

        private bool _disposed;
        private Hashtable _repositories;

        public RepositoyManager(DbContext context)
        {
            _context = new DbContextProxy(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();

            _disposed = true;
        }

        /// <summary>
        /// Get an IRepository for the specified type.
        /// If it has been already instanciated, it returns the instance.
        /// Otherwise, its will instanciate a new repository with the IDbContext as parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context.Target);

                _repositories.Add(type, repositoryInstance);
            }

            return _repositories[type] as IRepository<T>;
        }
    }
}
