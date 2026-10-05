namespace SMS.API.Models;

public class SalesDetail
{
    public int SalesDetailId { get; set; }
    public string InvoiceType { get; set; } = "INV";
    public int InvoiceNumber { get; set; }
    public int ItemId { get; set; }
    public decimal Qty { get; set; }
    public decimal Price { get; set; }
    public decimal Amount => Qty * Price;   // computed column on DB side too

    public SalesHeader? SalesHeader { get; set; }
    public Item? Item { get; set; }
}
