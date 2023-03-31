using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.EFCore;

namespace BookApi.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly RepositoryContext _context;

        public BooksController(RepositoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var books = _context.Books.ToList();
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var book = _context.Books.Find(id);
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

            _context.Books.Add(book);
            _context.SaveChanges();
            return StatusCode(201, book);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update([FromBody] Book book, [FromRoute(Name = "id")] int id)
        {
            var isExist = _context.Books.Any(x => x.Id == id);

            if (id != book.Id)
                return BadRequest("not matched id!");

            if (isExist)
            {
                _context.Entry(book).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(book);
            }
            return NotFound("Not find book!");
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteById([FromRoute(Name = "id")] int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
                return NoContent();
            }
            return NotFound("Not find book!");

        }

        [HttpPatch("{id:int}")]
        public IActionResult Patch([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            var entity = _context.Books.Find(id);
            if (entity is not null)
            {
                bookPatch.ApplyTo(entity);
                _context.SaveChanges();
                return NoContent();
            }
            return NotFound("not find book!");

        }
    }
}
