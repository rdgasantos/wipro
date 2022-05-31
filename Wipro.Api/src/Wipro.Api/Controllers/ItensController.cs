using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wipro.Business.Interfaces;
using Wipro.Business.Models;
using Wipro.Data.Repository;

namespace Wipro.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItensController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;

        public ItensController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Item>> GetItemFila()
        {
            var item = await _itemRepository.Get();

            if(item != null)
                return Ok(item);

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Item>> AddItemFila(List<Item> listItem)
        {
            await _itemRepository.Add(listItem);

            return Ok();
        }
    }
}
