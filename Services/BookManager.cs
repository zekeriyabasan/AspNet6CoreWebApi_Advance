using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        public BookManager(IRepositoryManager manager, ILoggerService logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public Book CreateABook(Book book)
        {
            _manager.Book.CreateABook(book);
            _manager.Save();
            return book;
        }

        public void DeleteABook(int id, bool trackChanges)
        {
            var exist = _manager.Book.GetABook(id, false);
            if (exist == null)
                throw new BookNotFoundException(id);
           
            _manager.Book.DeleteABook(exist);
            _manager.Save();
        }

        public Book GetABook(int id, bool trackChanges)
        {
            var exist = _manager.Book.GetABook(id, trackChanges);

            if (exist == null)
                throw new BookNotFoundException(id);

            return exist;
           
        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            return _manager.Book.GetAllBook(trackChanges);
        }

        public void UpdateABook(int id, Book book, bool trackChanges)
        {
            var exist = _manager.Book.GetABook(id, false);

            if (exist == null)
                throw new BookNotFoundException(id);

            exist.Name = book.Name;
            exist.Price = book.Price;
            _manager.Book.UpdateABook(exist);
            _manager.Save();
        }
    }
}
