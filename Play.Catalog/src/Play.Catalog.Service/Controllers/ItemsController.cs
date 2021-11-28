using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.Now),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.Now),
            new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deals a small amount of damage", 18, DateTimeOffset.Now)
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }


        [HttpGet("{id:guid}")]
        public ActionResult<ItemDto> GetById(Guid id)
        {
            ItemDto item = items.SingleOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
        {
            ItemDto item = new(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.Now);
            items.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
        {
            ItemDto existingItem = items.SingleOrDefault(x => x.Id == id);

            if (existingItem == null)
            {
                return NotFound();
            }

            ItemDto updatedItem = existingItem with
            {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price
            };

            int index = items.FindIndex(existingItem => existingItem.Id == id);
            items[index] = updatedItem;

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            int index = items.FindIndex(existingItem => existingItem.Id == id);

            if (index < 0)
            {
                return NotFound();
            }

            items.RemoveAt(index);
            return NoContent();
        }
    }
}