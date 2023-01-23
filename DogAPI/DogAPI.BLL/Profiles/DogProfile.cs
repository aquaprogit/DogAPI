using AutoMapper;

using DogAPI.Common.DTO;
using DogAPI.DAL.Entities;

namespace DogAPI.BLL.Profiles;
public class DogProfile : Profile
{
    public DogProfile()
    {
        CreateMap<Dog, DogDTO>().ReverseMap();
        CreateMap<Dog, UpdateDogDTO>().ReverseMap();
    }
}
