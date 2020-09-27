using Domain.Entities;
using Domain.Interfaces.Services;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Business
{
    public class PlaylistBusiness : IPlaylistBusiness
    {
        private readonly IPlaylistService _playlistService;
        private readonly IWeatherService _weatherService;

        public PlaylistBusiness(IPlaylistService playlistService, IWeatherService weatherService)
        {
            _playlistService = playlistService;
            _weatherService = weatherService;
        }

        public List<Playlist> GetByFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                throw new InvalidOperationException("City name is required");

            var muscialGenre = this.GetMusicalGenre(filter);

            return _playlistService.GetByFilter(muscialGenre);
        }

        private string GetMusicalGenre(string filter)
        {
            var weather = _weatherService.GetWeatherByFilter(filter);

            if (weather == null)
                throw new InvalidOperationException("Weather not found");

            if (weather.CurrentTemperature < 10)
                return "decades";
            else if (weather.CurrentTemperature >= 10 && weather.CurrentTemperature < 25)
                return "rock";
            else
                return "pop";
        }
    }
}
