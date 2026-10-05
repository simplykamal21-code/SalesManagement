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
public class CustomersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CustomersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll([FromQuery] string? search)
    {
        var query = _context.Customers.Where(c => c.IsActive).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.Name.Contains(search) || c.Code.Contains(search));
        }

        var customers = await query.OrderBy(c => c.Name).Select(c => new CustomerDto
        {
            CustomerId = c.CustomerId,
            Code = c.Code,
            Name = c.Name,
            Address = c.Address,
            Mobile = c.Mobile,
            Email = c.Email,
            Category = c.Category
        }).ToListAsync();

        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerDto>> GetById(int id)
    {
        var c = await _context.Customers.FindAsync(id);
        if (c is null || !c.IsActive) return NotFound();

        return Ok(new CustomerDto
        {
            CustomerId = c.CustomerId,
            Code = c.Code,
            Name = c.Name,
            Address = c.Address,
            Mobile = c.Mobile,
            Email = c.Email,
            Category = c.Category
        });
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CustomerCreateDto dto)
    {
        if (await _context.Customers.AnyAsync(c => c.Code == dto.Code))
        {
            return Conflict(new { message = $"Customer code '{dto.Code}' already exists." });
        }

        var customer = new Customer
        {
            Code = dto.Code,
            Name = dto.Name,
            Address = dto.Address,
            Mobile = dto.Mobile,
            Email = dto.Email,
            Category = dto.Category
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = customer.CustomerId }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CustomerCreateDto dto)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer is null || !customer.IsActive) return NotFound();

        customer.Name = dto.Name;
        customer.Address = dto.Address;
        customer.Mobile = dto.Mobile;
        customer.Email = dto.Email;
        customer.Category = dto.Category;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer is null) return NotFound();

        // Soft delete - keeps historical invoices intact
        customer.IsActive = false;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
