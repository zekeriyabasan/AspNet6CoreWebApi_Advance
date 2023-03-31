using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EFCore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book { Id = 1, Name = "Kitap 1", Price = 10 },
                new Book { Id = 2, Name = "Kitap 2", Price = 10 },
                new Book { Id = 3, Name = "Kitap 3", Price = 10 }
                );
        }
    }
}
