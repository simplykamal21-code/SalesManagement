using SMS.API.Models;

namespace SMS.API.Repositories.Interfaces;

public interface ISalesRepository
{
    Task<SalesHeader?> GetInvoiceAsync(string invoiceType, int invoiceNumber);
    Task<SalesHeader?> GetFirstAsync(string invoiceType);
    Task<SalesHeader?> GetLastAsync(string invoiceType);
    Task<SalesHeader?> GetNextAsync(string invoiceType, int currentNumber);
    Task<SalesHeader?> GetPriorAsync(string invoiceType, int currentNumber);
    Task<int> GetNextInvoiceNumberAsync(string invoiceType);
    Task<List<SalesHeader>> SearchAsync(string invoiceType, string? term);
    Task<List<SalesHeader>> ListAsync(string invoiceType);
    void AddHeader(SalesHeader header);
    void RemoveHeader(SalesHeader header);
    Task<int> SaveChangesAsync();
}
