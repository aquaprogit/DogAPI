using DogAPI.DAL.Entities;
using DogAPI.DAL.Repositories.Base;

namespace DogAPI.DAL.Repositories.Interfaces;
public interface IDogRepository : IRepo<Dog, string>
{
}
