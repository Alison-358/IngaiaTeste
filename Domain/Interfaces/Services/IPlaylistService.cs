using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Services
{
    public interface IPlaylistService : IBaseService<Playlist>
    {
        List<Playlist> GetByFilter(string filter);
    }
}
