namespace Entities.DataTransferObjects
{
    //[Serializable]
    //public record BookDto(int Id, string Name, decimal Price); 
    public record BookDto
    {
        public int Id { get; init; } // (init; yazdık çünkü )değeri nesne oluşturlurken verilmeli, dto lar readonly olmalı
        public string Name { get; init; }
        public decimal Price { get; init; }
    };
}
