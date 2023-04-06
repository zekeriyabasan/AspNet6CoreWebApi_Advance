using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBookRepository: IRepositoryBase<Book>
    {
        Task<IEnumerable<Book>> GetAllBookAsync(bool trackChanges);
        Task<Book> GetABookAsync(int id,bool trackChanges);
        void CreateABook(Book entity);
        void UpdateABook(Book entity);
        void DeleteABook(Book entity);
    }
}
