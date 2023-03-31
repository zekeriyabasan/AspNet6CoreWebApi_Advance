using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class // abstarct sınıf yaptık çünkü newlenmesin diye
    {
        protected readonly RepositoryContext _context; // protected yaptık çünkü kalıtım alan sınıflar erişsin diye

        protected RepositoryBase(RepositoryContext context)
        {
            _context = context;
        }

        public void Create(T entity) => _context.Set<T>().Add(entity);
        public void Delete(T entity) => _context.Set<T>().Remove(entity);

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition, bool trackChanges) =>
            !trackChanges ?
            _context.Set<T>().Where(condition).AsNoTracking() : // trackChanges false ise izleme
            _context.Set<T>().Where(condition); // trackChanges true ise izle


        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ?
            _context.Set<T>().AsNoTracking() : // trackChanges false ise izleme
            _context.Set<T>(); // trackChanges true ise izle

        public void Update(T entity) => _context.Set<T>().Update(entity);
    }
}
