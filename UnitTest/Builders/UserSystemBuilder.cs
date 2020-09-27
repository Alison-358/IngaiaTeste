using Bogus;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest.Builders
{
    public class UserSystemBuilder
    {
        private readonly Faker<UserSystem> _faker = new Faker<UserSystem>("pt_BR");

        private IEnumerable<UserSystem> Builder(int count)
        {
            var list = this._faker.RuleFor(p => p.Name, p => p.Name.FullName())
                                  .RuleFor(p => p.Email, p => p.Internet.Email())
                                  .RuleFor(p => p.RoleName, p => "admin")
                                  .RuleFor(p => p.Password, p => p.Internet.Password())
                                  .GenerateLazy(count);

            return list;
        }

        public IEnumerable<UserSystem> GetUserSystemBuilderList()
        {
            return this.Builder(50);
        }

        public UserSystem GetUserSystemBuilder()
        {
            return this.Builder(1).Last();
        }

        public IEnumerable<UserSystem> GetUserSystemBuilderEmptyList()
        {
            return new List<UserSystem>();
        }
    }
}
