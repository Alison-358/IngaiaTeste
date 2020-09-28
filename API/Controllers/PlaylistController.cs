using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Interfaces;
using Service.Utils.Exceptions;
using Service.Utils.Helper.Extensions;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize("Bearer")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistBusiness _playlistBusiness;
        private readonly IMemoryCache _cache;

        public PlaylistController(IPlaylistBusiness playlistBusiness, IMemoryCache cache)
        {
            _playlistBusiness = playlistBusiness;
            _cache = cache;
        }

        /// <summary>
        /// Find a Playlist by city name
        /// </summary>
        /// <remarks>Returns a array of Playlist</remarks>
        /// <param name="filter">Filter of Playlist to return. Required param</param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Invalid filter</response>
        /// <response code="404">Playlist not found</response>
        /// <response code="500">Internal exception</response>
        [HttpGet]
        [Route("/api/playlists")]
        public ActionResult<IEnumerable<Playlist>> Get([FromQuery] string filter)
        {
            try
            {
                var cacheEntry = new OkObjectResult(new { });

                if (!string.IsNullOrEmpty(filter))
                {
                    //taking data for query statistics
                    //Execute localhost
                    //var filePath = @"~/../Utils/Log/CitiesConsultationLog.txt";
                
                    //Execute heroku
                    var filePath = @"../../Utils/Log/CitiesConsultationLog.txt";

                    var playLists = new List<Playlist>();

                    var dateNow = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");

                    //removing accents
                    var asciiStr = filter.RemoveDiacritics().ToLower();

                    var line = "{ 'city' : '" + filter + "', 'count' : '1', 'date' : '" + dateNow + "', 'NameWithoutAccent': '" + asciiStr + "' }";
                    
                    //Cache
                    cacheEntry = _cache.GetOrCreate("MyCacheKey", entry =>
                    {
                        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                        entry.SetPriority(CacheItemPriority.High);

                        playLists = _playlistBusiness.GetByFilter(filter);

                        using (StreamWriter writer = new StreamWriter(filePath, true)) //// true to append data to the file
                        {
                            writer.WriteLine(line);
                        }

                        return Ok(playLists);
                    });
                    
                }

                return Ok(cacheEntry);
            }
            catch (ValidationException ve)
            {
                return StatusCode(400, ve.Message );
            }
            catch (NotFoundException nf)
            {
                return StatusCode(400, nf.Message );
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
