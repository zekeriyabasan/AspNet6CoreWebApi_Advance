using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace BookApi.Utilities.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<BookDtoForUpdate, Book>();
        }
    }
}
