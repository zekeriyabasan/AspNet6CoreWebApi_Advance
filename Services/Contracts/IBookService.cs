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
        IEnumerable<Book> GetAllBooks(bool trackChanges);
        Book GetABook(int id, bool trackChanges);
        Book CreateABook(Book book);
        void UpdateABook(int id, Book book, bool trackChanges);
        void DeleteABook(int id, bool trackChanges);
    }
}
