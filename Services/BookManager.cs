﻿using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.LinkModels;
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
        private readonly IBookLinks _bookLinks;
        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper, IBookLinks bookLinks)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
            _bookLinks = bookLinks;
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

        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooksAsync(LinkParameters linkParameters,bool trackChanges)
        {
            if (!linkParameters.BookParameters.IsValid)
                throw new PriceOutOfRangeBadRequestException();

            var booksWithPaged =await _manager.Book.GetAllBookAsync(linkParameters.BookParameters, trackChanges);

            var bookDtos = _mapper.Map<IEnumerable<BookDto>>(booksWithPaged);

            var links = _bookLinks.TryGenerateLinks(bookDtos, linkParameters.BookParameters.Fields, linkParameters.HttpContext);
            return (linkResponse: links, metaData: booksWithPaged.MetaData);
        }

        public async Task<List<Book>> GetAllBooksAsync(bool trackChanges)
        {
            var books = await _manager.Book.GetAllBookAsync(trackChanges);
            return books;
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
