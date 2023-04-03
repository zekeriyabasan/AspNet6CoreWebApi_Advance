namespace Entities.Exceptions
{
    public sealed class BookNotFoundException : NotFoundException // sealed ile bu class i kalıtım alınamaz hale getirdik
    {
        public BookNotFoundException(int id) 
            : base($"The book id : {id} could not found.")
        {
        }
    }
}
