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

        public async Task<BookDto> CreateABookAsync(BookDtoForInsertion book)
        {
            var entity = _mapper.Map<Book>(book);

            _manager.Book.CreateABook(entity);
            await _manager.SaveAsync();

            return _mapper.Map<BookDto>(entity);
        }

        public async Task DeleteABookAsync(int id, bool trackChanges)
        {
            var exist =await _manager.Book.GetABookAsync(id, false);
            if (exist == null)
                throw new BookNotFoundException(id);

            _manager.Book.DeleteABook(exist);
            await _manager.SaveAsync();
        }

        public async Task<BookDto> GetABookAsync(int id, bool trackChanges)
        {
            var exist = await _manager.Book.GetABookAsync(id, trackChanges);

            if (exist == null)
                throw new BookNotFoundException(id);

            return _mapper.Map<BookDto>(exist);

        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(bool trackChanges)
        {
            var books =await _manager.Book.GetAllBookAsync(trackChanges);

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            var book = await _manager.Book.GetABookAsync(id, trackChanges);

            if (book is null) throw new BookNotFoundException(id);
            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

            return (bookDtoForUpdate, book);
        }

        public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            await _manager.SaveAsync();
        }

        public async Task UpdateABookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
        {
            var exist =await _manager.Book.GetABookAsync(id, false);

            if (exist == null)
                throw new BookNotFoundException(id);

            exist = _mapper.Map<Book>(bookDto);
            _manager.Book.UpdateABook(exist);
            await _manager.SaveAsync();
        }
    }
}
