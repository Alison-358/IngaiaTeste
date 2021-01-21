using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class ForReactUseController : ControllerBase
    {
        private List<Obj> List()
        {
            var list = new List<Obj>();

            for(var i = 0; i <= 10; i++)
            {
                var obj = new Obj()
                {
                    Name = "Jose" + i,
                    Id = 1 + i,
                    LastName = "Cabral + 1" + i
                };

                list.Add(obj);
            }

            return list;
        }

        [HttpGet]
        [Route("/api/address")]
        public ActionResult<IEnumerable<Obj>> Get()
        {
            try
            {
                return base.StatusCode(200, this.List());
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

        [HttpGet("{id}")]
        [Route("/api/address/{id}")]
        public ActionResult<IEnumerable<object>> Get([FromRoute][Required] int id)
        {
            try
            {
                return base.StatusCode(200, this.List().Where(p => p.Id == id).ToList());
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

        [HttpPost]
        [Route("/api/address")]
        public ActionResult<Obj> Post([FromBody][Required] Obj body)
        {
            try
            {
                var id = this.List().Last().Id;

                body.Id = id + 1;

                this.List().Add(body);

                return base.StatusCode(200, body);
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

        [HttpPut]
        [Route("/api/address")]
        public ActionResult<Obj> Put([FromBody][Required] Obj body)
        {
            try
            {
                var listUpdated = this.List().Where(p => p.Id != body.Id).ToList();

                listUpdated.Add(body);

                return base.StatusCode(200, body);
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

        [HttpDelete]
        [Route("/api/address/{id}")]
        public ActionResult<Obj> Del([FromRoute][Required] int id)
        {
            try
            {
                var listUpdated = this.List().Where(p => p.Id != id).ToList();

                return base.StatusCode(200, listUpdated);
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

public class Obj
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
}