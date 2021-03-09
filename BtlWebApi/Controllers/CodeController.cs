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
    public class CodeController : ControllerBase
    {
        private readonly CodeService _codeService;

        public CodeController(CodeService codeService)
        {
            _codeService = codeService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Code> GetAll()
        {
            var codes = _codeService.GetAll();
            return codes;
        }

        [AllowAnonymous]
        [HttpGet("{id:length(24)}")]
        public Code Get(string id)
        {
            var code = _codeService.GetById(id);
            return code;
        }

    }
}
