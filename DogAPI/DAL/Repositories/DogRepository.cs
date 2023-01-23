using DogAPI.DAL.EF;
using DogAPI.DAL.Entities;
using DogAPI.DAL.Repositories.Base;
using DogAPI.DAL.Repositories.Interfaces;

namespace DogAPI.DAL.Repositories;
public class DogRepository : RepoBase<Dog, string>, IDogRepository
{
    public DogRepository(Context context) : base(context) { }
}
