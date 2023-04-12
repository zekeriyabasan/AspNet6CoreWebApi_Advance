using Entities.Models;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
namespace Repositories.EFCore.Extensions
{
    public static class BookRepositorExtensions
    {
        public static IQueryable<Book> FilterTheBooks(this IQueryable<Book> books, uint minPrice, uint maxPrice) =>
            books.Where(b => b.Price > minPrice && b.Price < maxPrice);

        public static IQueryable<Book> Search(this IQueryable<Book> books,string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return books;

            searchTerm = searchTerm.Trim().ToLower();
            return books.Where(b=>b.Name.ToLower().Contains(searchTerm));
        }

        public static IQueryable<Book> Sort(this IQueryable<Book> books, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return books.OrderBy(b=>b.Id);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Book>(orderByQueryString);

            if (orderQuery is null)
                return books.OrderBy(b => b.Id);

            return books.OrderBy(orderQuery); // kızıyorsa kütüphane kurulumu yapmamışsındır

        }
    }
}
