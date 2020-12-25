using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Utils.Exceptions;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize("Bearer")]
    public class CityLogController : ControllerBase
    {
        private readonly ILogger<CityLogController> _logger;

        public CityLogController(ILogger<CityLogController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Find a query statistic of cities
        /// </summary>
        /// <remarks>Returns a array of CityLog</remarks>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Invalid filter</response>
        /// <response code="404">Playlist not found</response>
        /// <response code="500">Internal exception</response>
        [HttpGet]
        [Route("/api/cityLog")]
        public ActionResult<IEnumerable<CityLog>> Get()
        {
            try
            {
                //var filePath = @"~/../Utils/Log/CitiesConsultationLog.txt";
                var filePath = @"../../Utils/Log/CitiesConsultationLog.txt";

                var cityies = new List<CityLog>();

                using (StreamReader file = new StreamReader(filePath))
                {
                    string ln;

                    while ((ln = file.ReadLine()) != null)
                    {
                        //teste
                        if (!string.IsNullOrEmpty(ln))
                        {
                            JObject json = JObject.Parse(ln);
                            var city = JsonConvert.DeserializeObject<CityLog>(json.ToString());
                            cityies.Add(city);
                        }
                    }
                    file.Close();
                }

                var result = cityies.GroupBy(a => a.NameWithoutAccent)
                                    .Select(g => new CityLog { NameWithoutAccent = g.Key, Count = g.Count() })
                                    .ToList();

                return Ok(result);
            }
            catch (ValidationException ve)
            {
                return StatusCode(400, ve);
            }
            catch (NotFoundException nf)
            {
                return StatusCode(400, nf);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex);
            }
        }
    }
}
