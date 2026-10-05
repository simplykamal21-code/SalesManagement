namespace SMS.API.DTOs;

public class ItemDto
{
    public int ItemId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ItemPhoto { get; set; }
    public decimal Cost { get; set; }
    public decimal SalesPrice { get; set; }
    public string? Unit { get; set; }
    public string? Brand { get; set; }
    public string? Supplier { get; set; }
}

public class ItemCreateDto
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ItemPhoto { get; set; }
    public decimal Cost { get; set; }
    public decimal SalesPrice { get; set; }
    public string? Unit { get; set; }
    public string? Brand { get; set; }
    public string? Supplier { get; set; }
}
