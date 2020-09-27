using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Service.Interfaces
{
    public interface IPlaylistBusiness
    {
        List<Playlist> GetByFilter(string filter);
    }
}
