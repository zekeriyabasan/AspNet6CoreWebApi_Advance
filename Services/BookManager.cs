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

        public BookDto CreateABook(BookDtoForInsertion book)
        {
            var entity = _mapper.Map<Book>(book);

            _manager.Book.CreateABook(entity);
            _manager.Save();

            return _mapper.Map<BookDto>(entity);
        }

        public void DeleteABook(int id, bool trackChanges)
        {
            var exist = _manager.Book.GetABook(id, false);
            if (exist == null)
                throw new BookNotFoundException(id);
           
            _manager.Book.DeleteABook(exist);
            _manager.Save();
        }

        public BookDto GetABook(int id, bool trackChanges)
        {
            var exist = _manager.Book.GetABook(id, trackChanges);

            if (exist == null)
                throw new BookNotFoundException(id);

            return _mapper.Map<BookDto>(exist);
           
        }

        public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
        {
            var books = _manager.Book.GetAllBook(trackChanges);

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public (BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges)
        {
            var book = _manager.Book.GetABook(id, trackChanges);

            if (book is null) throw new BookNotFoundException(id);
            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

            return (bookDtoForUpdate, book);
        }

        public void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate,book);
            _manager.Save();
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
