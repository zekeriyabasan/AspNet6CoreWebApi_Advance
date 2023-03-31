using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;

        public BookManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public Book CreateABook(Book book)
        {
            _manager.Book.CreateABook(book);
            _manager.Save();
            return book;
        }

        public void DeleteABook(int id, bool trackChanges)
        {
            var exist = _manager.Book.GetABook(id,false);
            if (exist == null)
            {
                throw new Exception($"book is not absent for {id}");

            }
            _manager.Book.DeleteABook(exist);
            _manager.Save();
        }

        public Book GetABook(int id, bool trackChanges)
        {
            return _manager.Book.GetABook(id,trackChanges);
        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            return _manager.Book.GetAllBook(trackChanges);
        }

        public void UpdateABook(int id, Book book, bool trackChanges)
        {
            var exist = _manager.Book.GetABook(id, false);
            if (exist == null)
            {
                throw new Exception($"book is not absent for {id}");
            }
            if(exist.Id != id)
                throw new Exception($"not matched ids {id}!={exist.Id}");

            if(book is null)
                throw new ArgumentNullException(nameof(book));

            exist.Name=book.Name;
            exist.Price=book.Price;
            _manager.Book.UpdateABook(exist);
            _manager.Save();
        }
    }
}
