using DogAPI.BLL.Profiles;
using DogAPI.DAL.EF;
using DogAPI.DAL.Repositories.Interfaces;
using DogAPI.DAL.Repositories;
using DogAPI.BLL.Services.Interfaces;
using DogAPI.BLL.Services;
using FluentValidation.AspNetCore;
using FluentValidation;
using DogAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

//Mappers
builder.Services.AddAutoMapper(typeof(DogProfile));

//DB context
builder.Services.AddDbContext<Context>();

//Repositories
builder.Services.AddScoped<IDogRepository, DogRepository>();

//Services
builder.Services.AddScoped<IDogService, DogService>();

//Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<DogValidator>();

//Controllers
builder.Services.AddControllers();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
