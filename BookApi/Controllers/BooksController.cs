using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore;

namespace BookApi.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        IRepositoryManager _manager;

        public BooksController(IRepositoryManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var books = _manager.Book.GetAllBook(false);
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var book = _manager.Book.GetABook(id,false);
            if (book is not null)
            {
                return Ok(book);
            }
            return NotFound("Not find book!");
           
        }

        [HttpPost]
        public IActionResult Add([FromBody] Book book)
        {
            if (book is null)
                return BadRequest();

            _manager.Book.CreateABook(book);
            _manager.Save();
            return StatusCode(201, book);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update([FromBody] Book book, [FromRoute(Name = "id")] int id)
        {
            var exist = _manager.Book.GetABook(id,true); // güncellerken izlemesi gerek trackChanges = true

            if (id != book.Id)
                return BadRequest("not matched id!");

            if (exist!=null)
            {
                _manager.Book.UpdateABook(book);
                _manager.Save();

                return Ok(book);
            }
            return NotFound("Not find book!");
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteById([FromRoute(Name = "id")] int id)
        {
            var book = _manager.Book.GetABook(id, false);
            if (book != null)
            {
                _manager.Book.DeleteABook(book);
                _manager.Save();
                return NoContent();
            }
            return NotFound("Not find book!");

        }

        [HttpPatch("{id:int}")]
        public IActionResult Patch([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            var entity = _manager.Book.GetABook(id, true);
            if (entity is not null)
            {
                bookPatch.ApplyTo(entity);
                _manager.Book.Update(entity);
                _manager.Save();
                return NoContent();
            }
            return NotFound("not find book!");

        }
    }
}
