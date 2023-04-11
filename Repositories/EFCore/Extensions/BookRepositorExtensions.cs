using Entities.Models;

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
    }
}
