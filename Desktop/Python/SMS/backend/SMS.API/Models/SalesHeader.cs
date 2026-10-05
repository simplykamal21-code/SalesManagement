namespace SMS.API.Models;

// Composite key (InvoiceType, InvoiceNumber) configured in ApplicationDbContext
public class SalesHeader
{
    public string InvoiceType { get; set; } = "INV";
    public int InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
    public int CustomerId { get; set; }
    public string? InvoiceDescription { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public Customer? Customer { get; set; }
    public ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();
}
