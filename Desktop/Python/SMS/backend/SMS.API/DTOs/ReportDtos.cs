namespace SMS.API.DTOs;

public class SalesReportRowDto
{
    public string ItemCode { get; set; } = string.Empty;
    public string ItemDescription { get; set; } = string.Empty;
    public string? ItemPhoto { get; set; }
    public string? Unit { get; set; }
    public string? CustomerName { get; set; }
    public string? Category { get; set; }
    public string? Brand { get; set; }
    public decimal Qty { get; set; }
    public decimal SaleAmount { get; set; }
    public decimal TotalCost { get; set; }
    public decimal GrossProfit { get; set; }
    public DateTime InvoiceDate { get; set; }
}

public class DashboardSummaryDto
{
    public decimal TotalSales { get; set; }
    public decimal TotalGrossProfit { get; set; }
    public string? TopSellingItem { get; set; }
    public List<SalesTrendPointDto> SalesTrend { get; set; } = new();
    public List<CategorySalesDto> SalesByCategory { get; set; } = new();
}

public class SalesTrendPointDto
{
    public string Period { get; set; } = string.Empty; // e.g. "2026-08"
    public decimal Amount { get; set; }
}

public class CategorySalesDto
{
    public string Category { get; set; } = "Uncategorized";
    public decimal Amount { get; set; }
}
