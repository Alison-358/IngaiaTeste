using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class LoginController : ControllerBase
    {
        private readonly IUserSystemBusiness _userSystemBusiness;

        public LoginController(IUserSystemBusiness userSystemBusiness)
        {
            _userSystemBusiness = userSystemBusiness;
        }

        [HttpPost]
        [AllowAnonymous]
        //[Authorize("Bearer")]
        [Route("/api/login")]
        public object PostAsync([FromBody][Required] UserSystem body)
        {
            try
            {
                if (body == null)
                    return BadRequest();

                var userCredentials = _userSystemBusiness.Login(body);

                return base.StatusCode(200, userCredentials);
            }
            catch (ValidationException ve)
            {
                return base.StatusCode(400, ve.Message);
            }
            catch (Exception ex)
            {
                return base.StatusCode(500, ex.Message);
            }
        }
    }
}