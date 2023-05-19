using Entities.DataTransferObjects;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookLinks : IBookLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<BookDto> _dataShaper;

        public BookLinks(IDataShaper<BookDto> dataShaper, LinkGenerator linkGenerator)
        {
            _dataShaper = dataShaper;
            _linkGenerator = linkGenerator;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<BookDto> bookDtos, string fields, HttpContext httpContext)
        {
            var shapedBooks = ShapedData(bookDtos, fields);
            // link üretebiliyor muyum?
            if (ShouldGenerateLinks(httpContext))
                return LinkedBooks(bookDtos, fields, httpContext, shapedBooks);

            return ReturnShapedBooks(shapedBooks);
        }

        private LinkResponse LinkedBooks(IEnumerable<BookDto> bookDtos, string fields, HttpContext httpContext, List<Entity> shapedBooks)
        {
            var bookDtoList = bookDtos.ToList();
            for (int index = 0; index < bookDtoList.Count(); index++)
            {
                var bookLinks = CreateForBook(httpContext, bookDtoList[index], fields);
                shapedBooks[index].Add("Links", bookLinks);
            }
            var bookCollection = new LinkCollectionWrapper<Entity>(shapedBooks);
            CreateForBooks(httpContext, bookCollection);
            return new LinkResponse { HasLinks = true, LinkedEntities = bookCollection };
        }
        private LinkCollectionWrapper<Entity> CreateForBooks(HttpContext httpContext,LinkCollectionWrapper<Entity> collectionWrapper)
        {
            collectionWrapper.Links.AddRange(new List<Link>() { 
                new Link()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}",
                    Rel="self",
                    Method="GET"
                },
                new Link()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}?pageSize=10&pageNumber=1&minPrice=11&maxPrice=799&searchTerm=a&OrderBy=Price&Fields=Price,name",
                    Rel="Filtering",
                    Method="GET"
                }
                });
            return collectionWrapper;
        }
        private List<Link> CreateForBook(HttpContext httpContext, BookDto bookDto, string fields)
        {
            var links = new List<Link>()
            {
                new Link()
                {
                 Href=$"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}/{bookDto.Id}",
                 Rel="self",
                 Method="GET"
                },
                new Link()
                {
                    Href=$"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}",
                    Rel="create",
                    Method="POST"
                }
            };
            return links;
        }

        private LinkResponse ReturnShapedBooks(List<Entity> shapedBooks)
        {
            return new LinkResponse { ShapedEntities = shapedBooks };
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType
                .SubTypeWithoutSuffix
                .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        private List<Entity> ShapedData(IEnumerable<BookDto> bookDtos, string fields)
        {
            return _dataShaper.GetShaperData(bookDtos, fields).Select(s => s.Entity).ToList();

        }
    }
}
