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
    public class ItemController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Item> GetAll()
        {
            var items = _itemService.GetAll();
            return items;
        }

        [AllowAnonymous]
        [HttpGet("{id:length(24)}")]
        public Item Get(string id)
        {
            var item =  _itemService.GetById(id);

            return item;
        }

    }
}
