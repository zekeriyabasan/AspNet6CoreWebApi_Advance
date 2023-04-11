using Entities.Models;

namespace Repositories.EFCore
{
    public static class BookRepositorExtensions
    {
        public static IQueryable<Book> FilterTheBooks(this IQueryable<Book> books, uint minPrice, uint maxPrice) =>
            books.Where(b => b.Price > minPrice && b.Price < maxPrice);
    }
}
