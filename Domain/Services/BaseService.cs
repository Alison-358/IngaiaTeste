using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
    }
}
