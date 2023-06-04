using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace BookApi.Utilities.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<BookDtoForUpdate, Book>().ReverseMap();
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<BookDtoForInsertion, Book>();  
            CreateMap<UserForRegistrationDto, User>();  
        }
    }
}
