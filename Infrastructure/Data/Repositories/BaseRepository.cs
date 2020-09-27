using Domain.Interfaces.Repositories;
using System;

namespace Infrastructure.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
    }
}
