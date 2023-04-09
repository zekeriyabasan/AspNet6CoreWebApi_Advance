using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Book> GetABookAsync(int id, bool trackChanges) => await FindByCondition(b => b.Id == id, trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Book>> GetAllBookAsync(BookParameters bookParameters,bool trackChanges) =>
            await FindAll(trackChanges).OrderBy(b=>b.Id).
            Skip((bookParameters.PageNumber-1)*bookParameters.PageSize).// sayfa numarası ve sayfa elaman satısına göre kaç eleman atlamalıyım
            Take(bookParameters.PageSize).ToListAsync(); // pagesize a göre kaç eleman almalıyız

        public void UpdateABook(Book entity) => Update(entity);
        
    }
}
