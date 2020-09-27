using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services
{
    public class PlaylistService : BaseService<Playlist>, IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistService(IPlaylistRepository playlistRepository)
            : base()
        {
            _playlistRepository = playlistRepository;
        }

        public List<Playlist> GetByFilter(string filter)
        {
            return _playlistRepository.GetByFilter(filter);
        }
    }
}
