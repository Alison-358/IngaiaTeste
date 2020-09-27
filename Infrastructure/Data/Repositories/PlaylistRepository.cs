using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.CrossCutting.Spotify;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Repositories
{
    public class PlaylistRepository : BaseRepository<Playlist>, IPlaylistRepository
    {
        public List<Playlist> GetByFilter(string filter)
        {
            var spotifay = new SpotifyAPI();

            var items = spotifay.GetPlaylistsByGenre(filter);

            return items.Result;
        }
    }
}
