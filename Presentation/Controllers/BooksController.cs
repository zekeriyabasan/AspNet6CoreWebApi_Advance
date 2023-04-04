﻿using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var books = _manager.BookService.GetAllBooks(false);
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var book = _manager.BookService.GetABook(id, false);

            return Ok(book);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Book book)
        {
            if (book is null)
                return BadRequest();

            _manager.BookService.CreateABook(book);
            return StatusCode(201, book);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update([FromBody] BookDtoForUpdate bookDto, [FromRoute(Name = "id")] int id)
        {
            if (bookDto is null)
                return BadRequest();

            if (bookDto.Id != id)
                throw new Exception($"not matched ids {id}!={bookDto.Id}");

            _manager.BookService.UpdateABook(id, bookDto, true);
            return Ok(bookDto);

        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteById([FromRoute(Name = "id")] int id)
        {
            _manager.BookService.DeleteABook(id, false);

            return NoContent();
        }



        [HttpPatch("{id:int}")]
        public IActionResult Patch([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            var entity = _manager.BookService.GetABook(id, false);
            if (entity is not null)
            {
                bookPatch.ApplyTo(entity);
                _manager.BookService.UpdateABook(id, new BookDtoForUpdate(entity.Id,entity.Name,entity.Price), true);
                return NoContent();
            }
            return NotFound("not find book!");

        }
    }
}
