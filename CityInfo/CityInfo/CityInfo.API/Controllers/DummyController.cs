using CityInfo.API.Contexts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/testdatabase")]
    public class DummyController : ControllerBase
    {
        private readonly CityInfoContext _ctx;

        public DummyController(CityInfoContext ctx) //injection
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }
        [HttpGet]  //decoration
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
