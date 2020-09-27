using Domain.Entities;
using Service.Interfaces;
using Service.Utils.Helper.LoginConfiguration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Business
{
    public class UserSystemBusiness : IUserSystemBusiness
    {
        private readonly Token _token;
        private readonly Signing _signing;

        public UserSystemBusiness(Token token, Signing signing)
        {
            _signing = signing;
            _token = token;
        }

        public Credentials Login(UserSystem userSystem)
        {
            LoginConfig loginConfig = new LoginConfig();

            return loginConfig.LoginCredentials(userSystem, _token, _signing);
        }
    }
}
