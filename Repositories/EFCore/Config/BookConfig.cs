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
                new Book { Id = 1, CategoryId = 1, Name = "Körlük", Price = 10 },
                new Book { Id = 2, CategoryId = 1, Name = "Jack London", Price = 10 },
                new Book { Id = 3, CategoryId = 2, Name = "Suç ve Ceza", Price = 10 }
                );
        }
    }
}
