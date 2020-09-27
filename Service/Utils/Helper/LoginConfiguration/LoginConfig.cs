using Domain.Entities;
using Service.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Service.Utils.Helper.LoginConfiguration
{
    public class LoginConfig
    {
        private UserSystem GetUserSystem(UserSystem userSystem)
        {
            try
            {
                if (userSystem == null)
                    throw new NotFoundException("User not found, check credentials");

                var users = new List<UserSystem>()
                {
                    new UserSystem(){
                        Email = "jose1@gmail.com",
                        Name = "José1",
                        Password = "jose123",
                        RoleName = "user"
                    },
                    new UserSystem(){
                        Email = "marcos101@gmail.com",
                        Name = "Marcos101",
                        Password = "marcos101",
                        RoleName = "admin"
                    }
                };

                return users.FirstOrDefault(p => p.Email == userSystem.Email && p.Password == userSystem.Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Credentials LoginCredentials(UserSystem user, Token _token, Signing _signing)
        {
            var userSystem = this.GetUserSystem(user);

            var credentialsUser = userSystem;

            bool credentialsValid;

            credentialsValid = credentialsUser != null ? true : false;

            if (credentialsValid)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(credentialsUser.Name, "Login"),
                    new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, credentialsUser.Name),
                        new Claim("Store", userSystem.RoleName),
                        new Claim(ClaimTypes.Email, credentialsUser.Email),
                        new Claim(ClaimTypes.Role, userSystem.RoleName),
                        //new Claim(ClaimTypes.Name, credentialsUser.UserName)
                    }
                    //CookieAuthenticationDefaults.AuthenticationScheme
                );

                DateTime createDate = DateTime.Now;
                DateTime expirationDate = createDate + TimeSpan.FromSeconds(_token.Seconds);

                var handler = new JwtSecurityTokenHandler();
                string token = CreateToken(identity, createDate, expirationDate, handler, _token, _signing);

                return CredentialsSuccess(createDate, expirationDate, token, identity);
            }
            else
            {
                return CredentialsException();
            }
        }

        private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler, Token _token, Signing _signing)
        {
            //Configuration Token
            var securityToken = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Issuer = _token.Issuer,
                Audience = _token.Audience,
                SigningCredentials = _signing.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate
            });

            var token = handler.WriteToken(securityToken);

            return token;
        }

        private Credentials CredentialsException()
        {
            return new Credentials
            {
                Authenticated = false,
                Message = "Authentication failed, check credentials"
            };
        }

        private Credentials CredentialsSuccess(DateTime createDate, DateTime expirationDate, string token, ClaimsIdentity identity)
        {
            return new Credentials()
            {
                Authenticated = true,
                Created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = token,
                Message = "Ok"
            };
        }
    }
}
