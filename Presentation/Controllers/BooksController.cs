using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using Presentation.ActionFilters;
using Services.Contracts;
using System.Text.Json;

namespace Presentation.Controllers
{
    [ServiceFilter(typeof(LogFilterAttribute))]
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]BookParameters bookParameters)
        {
            var pagedBooksResult = await _manager.BookService.GetAllBooksAsync(bookParameters,false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedBooksResult.Item2));

            return Ok(pagedBooksResult.Item1);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var book = await _manager.BookService.GetABookAsync(id, false);

            return Ok(book);
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BookDtoForInsertion bookDto)
        {
            var book = await _manager.BookService.CreateABookAsync(bookDto);
            return StatusCode(201, book);
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromBody] BookDtoForUpdate bookDto, [FromRoute(Name = "id")] int id)
        {
            if (bookDto.Id != id)
                throw new Exception($"not matched ids {id}!={bookDto.Id}");

            await _manager.BookService.UpdateABookAsync(id, bookDto, true);
            return Ok(bookDto);

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteById([FromRoute(Name = "id")] int id)
        {
            await _manager.BookService.DeleteABookAsync(id, false);

            return NoContent();
        }



        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {
            if (bookPatch is null) return BadRequest();
            //var bookDto = _manager.BookService.GetABook(id, false);
            var result =await _manager.BookService.GetOneBookForPatchAsync(id, false);

            bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);

            TryValidateModel(result.bookDtoForUpdate);

            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

           await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate,result.book);

            return NoContent(); // 204

        }
    }
}
