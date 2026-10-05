namespace SMS.API.DTOs;

public class SalesInvoiceDto
{
    public string InvoiceType { get; set; } = "INV";
    public int InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? InvoiceDescription { get; set; }
    public decimal TotalAmount { get; set; }
    public List<SalesLineDto> Lines { get; set; } = new();
}

public class SalesLineDto
{
    public int SalesDetailId { get; set; }
    public int ItemId { get; set; }
    public string? ItemCode { get; set; }
    public string? ItemDescription { get; set; }
    public string? Unit { get; set; }
    public decimal Qty { get; set; }
    public decimal Price { get; set; }
    public decimal Amount { get; set; }
}

// What the Angular grid actually posts back when saving an invoice
public class SalesInvoiceSaveDto
{
    public string InvoiceType { get; set; } = "INV";
    public int InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public int CustomerId { get; set; }
    public string? InvoiceDescription { get; set; }
    public List<SalesLineSaveDto> Lines { get; set; } = new();
}

public class SalesLineSaveDto
{
    public int ItemId { get; set; }
    public decimal Qty { get; set; }
    public decimal Price { get; set; }
}
