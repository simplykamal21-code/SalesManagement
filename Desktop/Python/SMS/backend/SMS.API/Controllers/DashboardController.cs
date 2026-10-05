using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.DTOs;

namespace SMS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
    {
        var details = await _context.SalesDetails
            .Include(d => d.Item)
            .Include(d => d.SalesHeader)
                .ThenInclude(h => h!.Customer)
            .ToListAsync();

        var totalSales = details.Sum(d => d.Qty * d.Price);
        var totalCost = details.Sum(d => d.Qty * d.Item!.Cost);
        var totalGrossProfit = totalSales - totalCost;

        var topItem = details
            .GroupBy(d => d.Item!.Description)
            .Select(g => new { Description = g.Key, Qty = g.Sum(x => x.Qty) })
            .OrderByDescending(x => x.Qty)
            .FirstOrDefault();

        var trend = details
            .GroupBy(d => new { d.SalesHeader!.InvoiceDate.Year, d.SalesHeader.InvoiceDate.Month })
            .Select(g => new SalesTrendPointDto
            {
                Period = $"{g.Key.Year:D4}-{g.Key.Month:D2}",
                Amount = g.Sum(x => x.Qty * x.Price)
            })
            .OrderBy(p => p.Period)
            .ToList();

        var byCategory = details
            .GroupBy(d => d.SalesHeader!.Customer!.Category ?? "Uncategorized")
            .Select(g => new CategorySalesDto
            {
                Category = g.Key,
                Amount = g.Sum(x => x.Qty * x.Price)
            })
            .OrderByDescending(c => c.Amount)
            .ToList();

        return Ok(new DashboardSummaryDto
        {
            TotalSales = totalSales,
            TotalGrossProfit = totalGrossProfit,
            TopSellingItem = topItem?.Description ?? "N/A",
            SalesTrend = trend,
            SalesByCategory = byCategory
        });
    }
}
