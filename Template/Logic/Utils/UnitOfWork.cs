using System;
using System.Linq;

namespace Logic.Utils
{
    public class UnitOfWork : IDisposable
    {
        private readonly DataDbContext _context;
        private bool _isAlive = true;
        private bool _isCommitted;

        public UnitOfWork(DataDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (!_isAlive)
                return;

            _isAlive = false;

            try
            {
                if (_isCommitted)
                {
                    _context.SaveChanges();
                }
            }
            finally
            {;
                _context.Dispose();
            }
        } 
        public void Commit()
        {
            if (!_isAlive)
                return;

            _isCommitted = true;
        }

        internal T Get<T>(long id) where T : class
        {
            return _context.Set<T>().Find(id);
        }

        internal void SaveOrUpdate<T>(T entity)
        {
            //_context.SaveChanges(entity);
        }

        internal void Delete<T>(T entity)
        {
            //_context.Set<T>().Remove(entity);
        }

        public IQueryable<T> Query<T>()
        {
            return null; //_session.Query<T>();
        }
    }
}
