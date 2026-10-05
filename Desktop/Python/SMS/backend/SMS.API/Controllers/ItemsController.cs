using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.DTOs;
using SMS.API.Models;

namespace SMS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ItemsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll([FromQuery] string? search)
    {
        var query = _context.Items.Where(i => i.IsActive).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(i => i.Description.Contains(search) || i.Code.Contains(search) || (i.Brand != null && i.Brand.Contains(search)));
        }

        var items = await query.OrderBy(i => i.Description).Select(i => new ItemDto
        {
            ItemId = i.ItemId,
            Code = i.Code,
            Description = i.Description,
            ItemPhoto = i.ItemPhoto,
            Cost = i.Cost,
            SalesPrice = i.SalesPrice,
            Unit = i.Unit,
            Brand = i.Brand,
            Supplier = i.Supplier
        }).ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemDto>> GetById(int id)
    {
        var i = await _context.Items.FindAsync(id);
        if (i is null || !i.IsActive) return NotFound();

        return Ok(new ItemDto
        {
            ItemId = i.ItemId,
            Code = i.Code,
            Description = i.Description,
            ItemPhoto = i.ItemPhoto,
            Cost = i.Cost,
            SalesPrice = i.SalesPrice,
            Unit = i.Unit,
            Brand = i.Brand,
            Supplier = i.Supplier
        });
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> Create(ItemCreateDto dto)
    {
        if (await _context.Items.AnyAsync(i => i.Code == dto.Code))
        {
            return Conflict(new { message = $"Item code '{dto.Code}' already exists." });
        }

        var item = new Item
        {
            Code = dto.Code,
            Description = dto.Description,
            ItemPhoto = dto.ItemPhoto,
            Cost = dto.Cost,
            SalesPrice = dto.SalesPrice,
            Unit = dto.Unit,
            Brand = dto.Brand,
            Supplier = dto.Supplier
        };

        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = item.ItemId }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ItemCreateDto dto)
    {
        var item = await _context.Items.FindAsync(id);
        if (item is null || !item.IsActive) return NotFound();

        item.Description = dto.Description;
        item.ItemPhoto = dto.ItemPhoto;
        item.Cost = dto.Cost;
        item.SalesPrice = dto.SalesPrice;
        item.Unit = dto.Unit;
        item.Brand = dto.Brand;
        item.Supplier = dto.Supplier;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item is null) return NotFound();

        item.IsActive = false;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
