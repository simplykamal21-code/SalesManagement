namespace SMS.API.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? Category { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public ICollection<SalesHeader> SalesHeaders { get; set; } = new List<SalesHeader>();
}
