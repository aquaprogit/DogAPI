﻿namespace DogAPI.BLL.Services.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(string password);
}
