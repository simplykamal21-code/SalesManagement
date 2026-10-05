using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.DTOs;

namespace SMS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Feeds the DevExtreme interactive grid on the front end - grouping/filtering happens
    // client-side once the flat rows are loaded, so we just return everything joined & flattened.
    [HttpGet("sales")]
    public async Task<ActionResult<IEnumerable<SalesReportRowDto>>> GetSalesReport(
        [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
    {
        var query = _context.SalesDetails
            .Include(d => d.Item)
            .Include(d => d.SalesHeader)
                .ThenInclude(h => h!.Customer)
            .AsQueryable();

        if (fromDate.HasValue)
            query = query.Where(d => d.SalesHeader!.InvoiceDate >= fromDate.Value);
        if (toDate.HasValue)
            query = query.Where(d => d.SalesHeader!.InvoiceDate <= toDate.Value);

        var rows = await query.Select(d => new SalesReportRowDto
        {
            ItemCode = d.Item!.Code,
            ItemDescription = d.Item.Description,
            ItemPhoto = d.Item.ItemPhoto,
            Unit = d.Item.Unit,
            Brand = d.Item.Brand,
            CustomerName = d.SalesHeader!.Customer!.Name,
            Category = d.SalesHeader.Customer.Category,
            Qty = d.Qty,
            SaleAmount = d.Qty * d.Price,
            TotalCost = d.Qty * d.Item.Cost,
            GrossProfit = (d.Qty * d.Price) - (d.Qty * d.Item.Cost),
            InvoiceDate = d.SalesHeader.InvoiceDate
        }).ToListAsync();

        return Ok(rows);
    }
}
