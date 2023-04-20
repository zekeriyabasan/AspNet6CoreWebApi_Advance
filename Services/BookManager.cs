using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;
using System.Dynamic;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<BookDto> _shaper;
        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper, IDataShaper<BookDto> shaper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
            _shaper = shaper;
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
            Book exist = await GetBookByIdExist(id, trackChanges);

            _manager.Book.DeleteABook(exist);
            await _manager.SaveAsync();
        }
        public async Task<BookDto> GetABookAsync(int id, bool trackChanges)
        {
            Book exist = await GetBookByIdExist(id,trackChanges);

            return _mapper.Map<BookDto>(exist);

        }

        public async Task<(IEnumerable<ShapedEntity>, MetaData)> GetAllBooksAsync(BookParameters bookParameters,bool trackChanges)
        {
            if (!bookParameters.IsValid)
                throw new PriceOutOfRangeBadRequestException();

            var booksWithPaged =await _manager.Book.GetAllBookAsync(bookParameters, trackChanges);

            var booksDto = _mapper.Map<IEnumerable<BookDto>>(booksWithPaged);

            var shapedData = _shaper.GetShaperData(booksDto,bookParameters.Fields);
            return (shapedData, booksWithPaged.MetaData);
        }

        public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            Book book = await GetBookByIdExist(id, trackChanges);
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
            Book exist = await GetBookByIdExist(id, trackChanges);

            exist = _mapper.Map<Book>(bookDto);
            _manager.Book.UpdateABook(exist);
            await _manager.SaveAsync();
        }

        private async Task<Book> GetBookByIdExist(int id,bool trackChanges)
        {
            var exist = await _manager.Book.GetABookAsync(id, trackChanges);
            if (exist == null)
                throw new BookNotFoundException(id);
            return exist;
        }
    }
}
