using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync(bool trackChanges);
        Task<BookDto> GetABookAsync(int id, bool trackChanges);
        Task<BookDto> CreateABookAsync(BookDtoForInsertion book);
        Task UpdateABookAsync(int id, BookDtoForUpdate book, bool trackChanges);
        Task DeleteABookAsync(int id, bool trackChanges);

        Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges);
        Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book);
    }
}
