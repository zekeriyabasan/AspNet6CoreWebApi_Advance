using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public sealed class BookRepository : RepositoryBase<Book>, IBookRepository // sealed ile zırhladık artık bu class ı kalıtım alamazlar
    {
        public BookRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateABook(Book entity) => Create(entity); // Methodlara RepositoryBase dekiler ile aynı isimleri vermedikki karışmasın

        public void DeleteABook(Book entity)=> Delete(entity);

        public async Task<Book> GetABookAsync(int id, bool trackChanges) => await FindByCondition(b => b.Id == id, trackChanges).SingleOrDefaultAsync();

        public async Task<PagedList<Book>> GetAllBookAsync(BookParameters bookParameters, bool trackChanges) 
        {
            var books = await FindAll(trackChanges)
                .FilterTheBooks(bookParameters.MinPrice,bookParameters.MaxPrice)
                .Search(bookParameters.SearchTerm)
                .Sort(bookParameters.OrderBy)
                .ToListAsync(); 

            return PagedList<Book>.ToPagedList(books,bookParameters.PageNumber, bookParameters.PageSize);
        }

        public async Task<List<Book>> GetAllBookAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).OrderBy(b => b.Id).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBookAsyncWithDetailsAsync(bool trackChanges)
        {
            return await _context.Books
                .Include(b=>b.Category) // Category de dolsun istiyoruz
                .OrderBy(b=>b.Id)
                .ToListAsync();   
        }

        public void UpdateABook(Book entity) => Update(entity);
        
    }
}
