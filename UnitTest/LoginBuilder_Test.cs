using System;
using System.Linq;
using UnitTest.Builders;
using Xunit;

namespace UnitTest
{
    public class UserSystemBuilder_Test
    {
        private readonly UserSystemBuilder _loginBuilder = new UserSystemBuilder();

        [Fact]
        public void Should_Invoke_A_User()
        {
            var user = this._loginBuilder.GetUserSystemBuilder();

            Assert.NotNull(user);
        }

        [Fact]
        public void Should_Invoke_A_Error_User_Null()
        {
            var user = this._loginBuilder.GetUserSystemBuilderEmptyList().FirstOrDefault(p => p.Email == "xx");

            Assert.NotNull(user);
        }

        [Fact]
        public void Should_Invoke_Email_And_Pasword_Not_Null()
        {
            var user = this._loginBuilder.GetUserSystemBuilder();

            Assert.Equal(string.IsNullOrEmpty(user.Email), string.IsNullOrEmpty(user.Password));
        }

        [Fact]
        public void Should_Invoke_A_Fail_Login()
        {
            var user = this._loginBuilder.GetUserSystemBuilder();

            var email = "";
            var pass = "";

            email = user.Email;
            pass = user.Password + "a";

            Assert.Equal(email, user.Email);
            Assert.Equal(pass, user.Password);
        }

        [Fact]
        public void Should_Invoke_A_Success_Login()
        {
            var user = this._loginBuilder.GetUserSystemBuilder();

            var email = "";
            var pass = "";

            email = user.Email;
            pass = user.Password;

            Assert.Equal(email, user.Email);
            Assert.Equal(pass, user.Password);
        }
    }
}
