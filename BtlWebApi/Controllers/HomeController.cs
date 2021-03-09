using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using BtlWebApi.Services;
using BtlWebApi.Models;

namespace BtlWebApi.Controllers
{
    // [Authorize]
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {

        public HomeController()
        {
           
        }

        [AllowAnonymous]
        [HttpGet()]
        public ActionResult<string> Get()
        {
            var resp = "{ \"whereami\": \"home page\" }";
            return resp;
        }

    }
}
