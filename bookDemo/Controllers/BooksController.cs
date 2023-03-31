using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace bookDemo.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(ApplicationContext.books);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute(Name = "id")] int id)
        {
            var book = ApplicationContext.books.SingleOrDefault(x => x.Id == id);
            if (book != null)
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

            ApplicationContext.books.Add(book);
            return StatusCode(201, book);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update([FromBody] Book book, [FromRoute(Name = "id")] int id)
        {
            var isExist = ApplicationContext.books.Any(x => x.Id == id);

            if (id != book.Id)
                return BadRequest("not matched id!");

            if (isExist)
            {
                ApplicationContext.books.Remove(ApplicationContext.books.FirstOrDefault(b=>b.Id==id));
                ApplicationContext.books.Add(book);
                return Ok(book);
            }
            return NotFound("Not find book!");
        }

        [HttpDelete]
        public IActionResult DeleteAll()
        {
            ApplicationContext.books.Clear();
            return NoContent();

        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteById([FromRoute(Name = "id")] int id)
        {
            var book = ApplicationContext.books.FirstOrDefault(x => x.Id == id);
            if (book != null)
            {
                ApplicationContext.books.Remove(book);
                return NoContent();
            }
            return NotFound("Not find book!");

        }

        [HttpPatch("{id:int}")]
        public IActionResult Patch([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            var entity = ApplicationContext.books.Find(b => b.Id.Equals(id));
            if (entity is not null)
            {
                bookPatch.ApplyTo(entity);
                return NoContent(); 
            }
            return NotFound("not find book!");

        }
    }
}
