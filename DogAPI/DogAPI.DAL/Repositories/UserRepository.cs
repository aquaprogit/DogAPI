using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DogAPI.DAL.EF;
using DogAPI.DAL.Entities;
using DogAPI.DAL.Repositories.Base;

using Microsoft.EntityFrameworkCore;

namespace DogAPI.DAL.Repositories;
public class UserRepository : RepoBase<User, int>
{
    public UserRepository(Context context) : base(context)
    {
    }
    public User? FindByLogin(string login)
    {
        return Table.FirstOrDefault(user => user.Login == login);
    }
    public async Task<User?> FindByLoginAsync(string login)
    {
        return await Table.FirstOrDefaultAsync(user => user.Login == login);
    }
}
