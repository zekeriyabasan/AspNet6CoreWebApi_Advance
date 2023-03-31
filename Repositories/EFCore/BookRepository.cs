using Entities.Models;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateABook(Book entity) => Create(entity); // Methodlara RepositoryBase dekiler ile aynı isimleri vermedikki karışmasın

        public void DeleteABook(Book entity)=> Delete(entity);

        public IQueryable<Book> GetABook(int id, bool trackChanges) => FindByCondition(b => b.Id == id, trackChanges).OrderBy(b=>b.Id);

        public IQueryable<Book> GetAllBook(bool trackChanges) => FindAll(trackChanges);

        public void UpdateABook(Book entity) => Update(entity);
        
    }
}
