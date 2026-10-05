namespace SMS.API.Models;

public class Item
{
    public int ItemId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ItemPhoto { get; set; }   // Base64 or URL
    public decimal Cost { get; set; }
    public decimal SalesPrice { get; set; }
    public string? Unit { get; set; }
    public string? Brand { get; set; }
    public string? Supplier { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}
