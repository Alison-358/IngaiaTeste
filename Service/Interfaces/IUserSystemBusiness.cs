using Domain.Entities;
using Service.Utils.Helper.LoginConfiguration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interfaces
{
    public interface IUserSystemBusiness
    {
        Credentials Login(UserSystem userSystem);
    }
}
