using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Repositories
{
    public interface IPlaylistRepository : IBaseRepository<Playlist>
    {
        List<Playlist> GetByFilter(string filter);
    }
}
