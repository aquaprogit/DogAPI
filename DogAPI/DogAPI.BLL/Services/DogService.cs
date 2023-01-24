using System.Reflection;

using AutoMapper;

using DogAPI.BLL.Services.Interfaces;
using DogAPI.Common.DTO;
using DogAPI.Common.Extensions;
using DogAPI.DAL.Entities;
using DogAPI.DAL.Repositories.Interfaces;

namespace DogAPI.BLL.Services;

public class DogService : IDogService
{
    private readonly IDogRepository _repository;
    private readonly IMapper _mapper;

    public DogService(IDogRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DogDTO> AddDog(DogDTO dog)
    {
        Dog entity = _mapper.Map<Dog>(dog);
        if (await _repository.Table.FindAsync(entity.Name) != null)
            throw new InvalidOperationException("Entity with such key already exists in database");

        await _repository.AddAsync(entity);
        return _mapper.Map<DogDTO>(entity);
    }

    public async Task<bool> DeleteDog(string name)
    {
        var dog = await _repository.FindAsync(name);
        return dog != null && await _repository.DeleteAsync(dog) > 0;
    }

    public async Task<DogDTO?> GetDogByName(string name)
    {
        Dog? dog = await _repository.FindAsync(name);
        return dog != null ? _mapper.Map<DogDTO>(dog) : null;
    }

    public List<DogDTO> GetDogs()
    {
        return _mapper.Map<IEnumerable<DogDTO>>(_repository.GetAll()).ToList();
    }

    public List<DogDTO> GetDogs(string attribute, string? order)
    {
        var source = _repository.GetAll();

        source = SortByAttribute(attribute, order, source);

        return _mapper.Map<IEnumerable<DogDTO>>(source).ToList();
    }

    public List<DogDTO> GetDogs(string attribute, string? order, int pageNumber, int pageSize)
    {
        var source = SortByAttribute(attribute, order, _repository.GetAll()).ToList();
        int totalCount = source.Count;
        if (pageSize > totalCount || pageNumber < 1 || pageSize < 1)
            return _mapper.Map<IEnumerable<DogDTO>>(source).ToList();

        source = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return _mapper.Map<IEnumerable<DogDTO>>(source).ToList();
    }

    public async Task<DogDTO> UpdateDog(string name, UpdateDogDTO dog)
    {
        var entity = await _repository.FindAsync(name);

        if (entity == null)
        {
            throw new KeyNotFoundException($"Unable to find entity with such key {name}");
        }

        _mapper.Map(dog, entity);

        await _repository.UpdateAsync(entity);

        return _mapper.Map<DogDTO>(entity);
    }

    private static IEnumerable<Dog> SortByAttribute(string attribute, string? order, IEnumerable<Dog> source)
    {
        PropertyInfo? prop = typeof(Dog).GetProperty(attribute.Capitalize());
        if (prop == null)
            throw new InvalidOperationException($"Not found such property as {attribute.Capitalize()}");

        if (order == null || order.ToLower() == "asc")
            source = source.OrderBy(d => prop.GetValue(d, null)).ToList();
        else if (order.ToLower() == "desc")
            source = source.OrderByDescending(d => prop.GetValue(d, null)).ToList();
        return source;
    }
}