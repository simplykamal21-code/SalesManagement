using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.Models;
using SMS.API.Repositories.Interfaces;

namespace SMS.API.Repositories;

public class SalesRepository : ISalesRepository
{
    private readonly ApplicationDbContext _context;

    public SalesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SalesHeader?> GetInvoiceAsync(string invoiceType, int invoiceNumber) =>
        await _context.SalesHeaders
            .Include(h => h.Customer)
            .Include(h => h.SalesDetails).ThenInclude(d => d.Item)
            .FirstOrDefaultAsync(h => h.InvoiceType == invoiceType && h.InvoiceNumber == invoiceNumber);

    public async Task<SalesHeader?> GetFirstAsync(string invoiceType) =>
        await _context.SalesHeaders
            .Where(h => h.InvoiceType == invoiceType)
            .OrderBy(h => h.InvoiceNumber)
            .FirstOrDefaultAsync();

    public async Task<SalesHeader?> GetLastAsync(string invoiceType) =>
        await _context.SalesHeaders
            .Where(h => h.InvoiceType == invoiceType)
            .OrderByDescending(h => h.InvoiceNumber)
            .FirstOrDefaultAsync();

    public async Task<SalesHeader?> GetNextAsync(string invoiceType, int currentNumber) =>
        await _context.SalesHeaders
            .Where(h => h.InvoiceType == invoiceType && h.InvoiceNumber > currentNumber)
            .OrderBy(h => h.InvoiceNumber)
            .FirstOrDefaultAsync();

    public async Task<SalesHeader?> GetPriorAsync(string invoiceType, int currentNumber) =>
        await _context.SalesHeaders
            .Where(h => h.InvoiceType == invoiceType && h.InvoiceNumber < currentNumber)
            .OrderByDescending(h => h.InvoiceNumber)
            .FirstOrDefaultAsync();

    public async Task<int> GetNextInvoiceNumberAsync(string invoiceType)
    {
        var max = await _context.SalesHeaders
            .Where(h => h.InvoiceType == invoiceType)
            .Select(h => (int?)h.InvoiceNumber)
            .MaxAsync();
        return (max ?? 1000) + 1;
    }

    public async Task<List<SalesHeader>> SearchAsync(string invoiceType, string? term)
    {
        var query = _context.SalesHeaders
            .Include(h => h.Customer)
            .Where(h => h.InvoiceType == invoiceType);

        if (!string.IsNullOrWhiteSpace(term))
        {
            query = query.Where(h =>
                h.InvoiceNumber.ToString().Contains(term) ||
                (h.Customer != null && h.Customer.Name.Contains(term)) ||
                (h.InvoiceDescription != null && h.InvoiceDescription.Contains(term)));
        }

        return await query.OrderByDescending(h => h.InvoiceNumber).ToListAsync();
    }

    public async Task<List<SalesHeader>> ListAsync(string invoiceType) =>
        await _context.SalesHeaders
            .Include(h => h.Customer)
            .Where(h => h.InvoiceType == invoiceType)
            .OrderByDescending(h => h.InvoiceNumber)
            .ToListAsync();

    public void AddHeader(SalesHeader header) => _context.SalesHeaders.Add(header);

    public void RemoveHeader(SalesHeader header) => _context.SalesHeaders.Remove(header);

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
