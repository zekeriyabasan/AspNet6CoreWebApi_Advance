using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace BookApi.Utilities.Formatters
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv")); // gelen isteğin bu tipte bekliyorum demek.
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type? type)
        {
            // Gelen tipe bakıyorum ona göre Bookdto yada listesi mi
            if (typeof(BookDto).IsAssignableFrom(type) || typeof(IEnumerable<BookDto>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }

        private static void FormatCsv(StringBuilder buffer, BookDto book)
        {
            buffer.AppendLine($"{book.Id}, {book.Name}, {book.Price}");
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            if (context.Object is IEnumerable<BookDto>) // objeden çıkartma işlemi ile formatter ı nasış çalıştıracağımıza karar veriyoruz.
            {
                foreach (var book in (IEnumerable<BookDto>)context.Object) // nesneyi kutudan çıkardık
                {
                    FormatCsv(buffer, book);
                }
            }
            else
            {
                FormatCsv(buffer, (BookDto)context.Object);
            }

            await response.WriteAsync(buffer.ToString());
        }
    }
}
