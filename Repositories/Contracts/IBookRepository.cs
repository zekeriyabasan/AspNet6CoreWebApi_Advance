using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBookRepository: IRepositoryBase<Book>
    {
        Task<PagedList<Book>> GetAllBookAsync(BookParameters bookParameters,bool trackChanges);
        Task<List<Book>> GetAllBookAsync(bool trackChanges);
        Task<Book> GetABookAsync(int id,bool trackChanges);
        void CreateABook(Book entity);
        void UpdateABook(Book entity);
        void DeleteABook(Book entity);
        Task<IEnumerable<Book>> GetAllBookAsyncWithDetailsAsync(bool trackChanges);

    }
}
