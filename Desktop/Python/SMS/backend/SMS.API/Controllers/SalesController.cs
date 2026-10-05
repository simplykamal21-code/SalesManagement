using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.API.DTOs;
using SMS.API.Models;
using SMS.API.Repositories.Interfaces;

namespace SMS.API.Controllers;

// Backs the Sales Invoice Entry screen - handles the Top/Prior/Next/Bottom/Search/List
// navigation bar plus Add/Edit/Delete/Save/Cancel for the transaction itself.
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISalesRepository _salesRepo;

    public SalesController(ISalesRepository salesRepo)
    {
        _salesRepo = salesRepo;
    }

    [HttpGet("next-number")]
    public async Task<ActionResult<int>> GetNextInvoiceNumber([FromQuery] string invoiceType = "INV")
    {
        var next = await _salesRepo.GetNextInvoiceNumberAsync(invoiceType);
        return Ok(next);
    }

    [HttpGet("{invoiceType}/{invoiceNumber:int}")]
    public async Task<ActionResult<SalesInvoiceDto>> GetInvoice(string invoiceType, int invoiceNumber)
    {
        var header = await _salesRepo.GetInvoiceAsync(invoiceType, invoiceNumber);
        if (header is null) return NotFound();

        return Ok(MapToDto(header));
    }

    // ---- Navigation bar endpoints ----

    [HttpGet("{invoiceType}/first")]
    public async Task<ActionResult<SalesInvoiceDto>> First(string invoiceType)
    {
        var header = await _salesRepo.GetFirstAsync(invoiceType);
        return header is null ? NotFound() : Ok(MapToDto(header));
    }

    [HttpGet("{invoiceType}/last")]
    public async Task<ActionResult<SalesInvoiceDto>> Last(string invoiceType)
    {
        var header = await _salesRepo.GetLastAsync(invoiceType);
        return header is null ? NotFound() : Ok(MapToDto(header));
    }

    [HttpGet("{invoiceType}/{currentNumber:int}/next")]
    public async Task<ActionResult<SalesInvoiceDto>> Next(string invoiceType, int currentNumber)
    {
        var header = await _salesRepo.GetNextAsync(invoiceType, currentNumber);
        return header is null ? NotFound() : Ok(MapToDto(header));
    }

    [HttpGet("{invoiceType}/{currentNumber:int}/prior")]
    public async Task<ActionResult<SalesInvoiceDto>> Prior(string invoiceType, int currentNumber)
    {
        var header = await _salesRepo.GetPriorAsync(invoiceType, currentNumber);
        return header is null ? NotFound() : Ok(MapToDto(header));
    }

    [HttpGet("{invoiceType}/search")]
    public async Task<ActionResult<IEnumerable<object>>> Search(string invoiceType, [FromQuery] string? term)
    {
        var results = await _salesRepo.SearchAsync(invoiceType, term);
        return Ok(results.Select(h => new
        {
            h.InvoiceType,
            h.InvoiceNumber,
            h.InvoiceDate,
            CustomerName = h.Customer?.Name,
            h.TotalAmount
        }));
    }

    [HttpGet("{invoiceType}/list")]
    public async Task<ActionResult<IEnumerable<object>>> List(string invoiceType)
    {
        var results = await _salesRepo.ListAsync(invoiceType);
        return Ok(results.Select(h => new
        {
            h.InvoiceType,
            h.InvoiceNumber,
            h.InvoiceDate,
            CustomerName = h.Customer?.Name,
            h.TotalAmount
        }));
    }

    // ---- Save (Add or Edit) ----

    [HttpPost]
    public async Task<ActionResult<SalesInvoiceDto>> Save(SalesInvoiceSaveDto dto)
    {
        if (dto.Lines.Count == 0)
        {
            return BadRequest(new { message = "An invoice needs at least one line item." });
        }

        var existing = await _salesRepo.GetInvoiceAsync(dto.InvoiceType, dto.InvoiceNumber);

        if (existing is null)
        {
            // Add mode
            var header = new SalesHeader
            {
                InvoiceType = dto.InvoiceType,
                InvoiceNumber = dto.InvoiceNumber,
                InvoiceDate = dto.InvoiceDate,
                CustomerId = dto.CustomerId,
                InvoiceDescription = dto.InvoiceDescription,
                TotalAmount = dto.Lines.Sum(l => l.Qty * l.Price)
            };

            foreach (var line in dto.Lines)
            {
                header.SalesDetails.Add(new SalesDetail
                {
                    ItemId = line.ItemId,
                    Qty = line.Qty,
                    Price = line.Price
                });
            }

            _salesRepo.AddHeader(header);
        }
        else
        {
            // Edit mode - replace the line items wholesale, simplest to reason about
            existing.InvoiceDate = dto.InvoiceDate;
            existing.CustomerId = dto.CustomerId;
            existing.InvoiceDescription = dto.InvoiceDescription;
            existing.TotalAmount = dto.Lines.Sum(l => l.Qty * l.Price);

            existing.SalesDetails.Clear();
            foreach (var line in dto.Lines)
            {
                existing.SalesDetails.Add(new SalesDetail
                {
                    ItemId = line.ItemId,
                    Qty = line.Qty,
                    Price = line.Price
                });
            }
        }

        await _salesRepo.SaveChangesAsync();

        var saved = await _salesRepo.GetInvoiceAsync(dto.InvoiceType, dto.InvoiceNumber);
        return Ok(MapToDto(saved!));
    }

    [HttpDelete("{invoiceType}/{invoiceNumber:int}")]
    public async Task<IActionResult> Delete(string invoiceType, int invoiceNumber)
    {
        var header = await _salesRepo.GetInvoiceAsync(invoiceType, invoiceNumber);
        if (header is null) return NotFound();

        _salesRepo.RemoveHeader(header);
        await _salesRepo.SaveChangesAsync();
        return NoContent();
    }

    private static SalesInvoiceDto MapToDto(SalesHeader header) => new()
    {
        InvoiceType = header.InvoiceType,
        InvoiceNumber = header.InvoiceNumber,
        InvoiceDate = header.InvoiceDate,
        CustomerId = header.CustomerId,
        CustomerName = header.Customer?.Name,
        InvoiceDescription = header.InvoiceDescription,
        TotalAmount = header.TotalAmount,
        Lines = header.SalesDetails.Select(d => new SalesLineDto
        {
            SalesDetailId = d.SalesDetailId,
            ItemId = d.ItemId,
            ItemCode = d.Item?.Code,
            ItemDescription = d.Item?.Description,
            Unit = d.Item?.Unit,
            Qty = d.Qty,
            Price = d.Price,
            Amount = d.Qty * d.Price
        }).ToList()
    };
}
