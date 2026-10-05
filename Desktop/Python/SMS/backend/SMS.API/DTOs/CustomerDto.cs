namespace SMS.API.DTOs;

public class CustomerDto
{
    public int CustomerId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? Category { get; set; }
}

public class CustomerCreateDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? Category { get; set; }
}
