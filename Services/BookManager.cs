using AutoMapper;
using Entities.DataTransferObjects;
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
        private readonly IMapper _mapper;
        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
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

        public void UpdateABook(int id, BookDtoForUpdate bookDto, bool trackChanges)
        {
            var exist = _manager.Book.GetABook(id, false);

            if (exist == null)
                throw new BookNotFoundException(id);

            exist = _mapper.Map<Book>(bookDto);
            _manager.Book.UpdateABook(exist);
            _manager.Save();
        }
    }
}
