using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using Presentation.ActionFilters;
using Services.Contracts;
using System.Text.Json;

namespace Presentation.Controllers
{
    //[ApiVersion("1.0")]
    [ServiceFilter(typeof(LogFilterAttribute))]
    [Route("api/books")]
    [ApiController]
    //[ResponseCache(CacheProfileName ="5mins")]
    public class BooksController : ControllerBase
    {
        IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [Authorize]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [HttpGet(Name ="GetAllBooksAsync")]
        //[ResponseCache(Duration =60)]
        //[HttpCacheExpiration(CacheLocation = CacheLocation.Public,MaxAge =10)] // extension metoddaki config i override ettik
        public async Task<IActionResult> GetAllBooksAsync([FromQuery] BookParameters bookParameters)
        {
            var linkParameters = new LinkParameters { BookParameters = bookParameters, HttpContext = HttpContext };

            var result = await _manager.BookService.GetAllBooksAsync(linkParameters, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));

            return result.linkResponse.HasLinks ?
                Ok(result.linkResponse.LinkedEntities)
                : Ok(result.linkResponse.ShapedEntities);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var book = await _manager.BookService.GetABookAsync(id, false);

            return Ok(book);
        }

        [Authorize(Roles = "Admin,Editor")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost(Name = "CreateABookAsync")]
        public async Task<IActionResult> CreateABookAsync([FromBody] BookDtoForInsertion bookDto)
        {
            var book = await _manager.BookService.CreateABookAsync(bookDto);
            return StatusCode(201, book);
        }

        [Authorize(Roles = "Admin,Editor")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromBody] BookDtoForUpdate bookDto, [FromRoute(Name = "id")] int id)
        {
            if (bookDto.Id != id)
                throw new Exception($"not matched ids {id}!={bookDto.Id}");

            await _manager.BookService.UpdateABookAsync(id, bookDto, true);
            return Ok(bookDto);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteById([FromRoute(Name = "id")] int id)
        {
            await _manager.BookService.DeleteABookAsync(id, false);

            return NoContent();
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {
            if (bookPatch is null) return BadRequest();
            //var bookDto = _manager.BookService.GetABook(id, false);
            var result = await _manager.BookService.GetOneBookForPatchAsync(id, false);

            bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);

            TryValidateModel(result.bookDtoForUpdate);

            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate, result.book);

            return NoContent(); // 204

        }

        [Authorize]
        [HttpOptions]
        public IActionResult GetBooksOptions()
        {
            Response.Headers.Add("Allow", "GET, PUT, POST, PATCH, DELETE, HEAD, OPTIONS");
            return Ok();
        }
    }
}
