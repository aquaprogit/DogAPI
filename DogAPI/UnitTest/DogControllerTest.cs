using AutoMapper;

using DogAPI.BLL.Profiles;
using DogAPI.BLL.Services;
using DogAPI.Common.DTO;
using DogAPI.Controllers;
using DogAPI.DAL.Entities;
using DogAPI.DAL.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

using Moq;

namespace UnitTest;

public class DogControllerTest
{
    private readonly Mock<IDogRepository> _mockRepo;
    private readonly DogController _controller;
    private readonly IMapper _mapper;

    public DogControllerTest()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<DogProfile>()).CreateMapper();
        _mockRepo = new Mock<IDogRepository>();
        _controller = new DogController(new DogService(_mockRepo.Object, _mapper));
    }

    [Fact]
    public void Dogs_GetAll_ModelEqualityTest()
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetAll())
            .Returns(new List<Dog>() { new Dog()
            {
                Name = "Ayala",
                Color = "white",
                TailLength = 10,
                Weight = 12
            },
            new Dog() {
                Name = "Bobby",
                Color = "black",
                TailLength = 11,
                Weight = 13
            }
            });
        //Act
        var result = _controller.GetAll() as OkObjectResult;
        List<DogDTO> content = result?.Value as List<DogDTO> ?? new List<DogDTO>();
        //Assert
        Assert.IsType<List<DogDTO>>(content);
        Assert.Equal(2, content.Count);
        Assert.Equal("Ayala", content[0].Name);
        Assert.Equal("white", content[0].Color);
        Assert.Equal(10, content[0].TailLength);
        Assert.Equal(12, content[0].Weight);
        Assert.Equal("Bobby", content[1].Name);
        Assert.Equal("black", content[1].Color);
        Assert.Equal(11, content[1].TailLength);
        Assert.Equal(13, content[1].Weight);
    }
    [Fact]
    public void Dogs_GetAll_SortingDescTest()
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetAll()).Returns(new List<Dog>() { new Dog()
            {
                Name = "Neo",
                Color = "white",
                TailLength = 10,
                Weight = 12
            },
            new Dog() {
                Name = "Jessy",
                Color = "black",
                TailLength = 11,
                Weight = 13
            },
            new Dog() {
                Name = "Bob",
                Color = "black",
                TailLength = 33,
                Weight = 2
            }
            });
        //Act
        var result = _controller.GetAll("tailLength", "desc", null, null) as OkObjectResult;
        var content = result?.Value as List<DogDTO> ?? new List<DogDTO>();

        //Assert
        Assert.IsType<List<DogDTO>>(content);
        Assert.Equal(3, content.Count);
        Assert.Equal("Bob", content[0].Name);
        Assert.Equal(33, content[0].TailLength);
        Assert.Equal("Jessy", content[1].Name);
        Assert.Equal(11, content[1].TailLength);
        Assert.Equal("Neo", content[2].Name);
        Assert.Equal(10, content[2].TailLength);
    }
    [Fact]
    public void Dogs_GetAll_SortingAscTest()
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetAll()).Returns(new List<Dog>() { new Dog()
            {
                Name = "Clinton",
                Color = "white",
                TailLength = 10,
                Weight = 12
            },
            new Dog() {
                Name = "Bobby",
                Color = "black",
                TailLength = 11,
                Weight = 13
            },
            new Dog() {
                Name = "Ayala",
                Color = "black",
                TailLength = 33,
                Weight = 2
            }
            });
        //Act
        var result = (OkObjectResult)_controller.GetAll("name", "asc", null, null);
        var content = result.Value as List<DogDTO> ?? new List<DogDTO>();

        //Assert
        Assert.IsType<List<DogDTO>>(content);
        Assert.Equal(3, content.Count);
        Assert.Equal("Ayala", content[0].Name);
        Assert.Equal("Bobby", content[1].Name);
        Assert.Equal("Clinton", content[2].Name);
    }
    [Fact]
    public void Dogs_GetAll_PaginationIsValid()
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetAll()).Returns(new List<Dog>() {
            new Dog() {
                Name = "Ayala",
                Color = "white",
                TailLength = 10,
                Weight = 12
            },
            new Dog() {
                Name = "Bobby",
                Color = "black",
                TailLength = 11,
                Weight = 13
            },
            new Dog() {
                Name = "Clinton",
                Color = "black",
                TailLength = 33,
                Weight = 2
            },
            new Dog() {
                Name = "Dodge",
                Color = "brown",
                TailLength = 11,
                Weight = 22
            },
            new Dog() {
                Name = "Eliot",
                Color = "magenta",
                TailLength = 1,
                Weight = 9
            }
            });
        //Act
        var result = _controller.GetAll(null, null, 2, 2) as OkObjectResult;
        var content = result?.Value as List<DogDTO> ?? new List<DogDTO>();

        //Assert
        Assert.NotEqual(new List<DogDTO>(), content);
        Assert.Equal(2, content.Count);
        Assert.Equal("Clinton", content[0].Name);
        Assert.Equal("Dodge", content[1].Name);
    }
}