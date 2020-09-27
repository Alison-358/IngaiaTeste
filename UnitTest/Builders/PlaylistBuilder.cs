using Bogus;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest.Builders
{
    public class PlaylistBuilder
    {
        private readonly Faker<Playlist> _faker = new Faker<Playlist>("pt_BR");
        private readonly Playlist _playlist = new Playlist();

        private IEnumerable<Playlist> Builder(int count)
        {
            var list = this._faker.RuleFor(p => p.Name, p => p.Music.Genre())
                                  .GenerateLazy(count);

            return list;
        }

        public IEnumerable<Playlist> GetPlaylistBuilderList()
        {
            return this.Builder(50);
        }

        public Playlist GetPlaylistBuilder()
        {
            return this.Builder(1).Last();
        }

        public IEnumerable<Playlist> GetPlaylistBuilderEmptyList()
        {
            return new List<Playlist>();
        }
    }
}
