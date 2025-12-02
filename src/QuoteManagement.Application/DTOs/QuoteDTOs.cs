namespace QuoteManagement.Application.DTOs;

public class QuoteRequestDto
{
    public string Mode { get; set; } = string.Empty;
    public string MovementType { get; set; } = string.Empty;
    public string Incoterms { get; set; } = string.Empty;
    public string PackageType { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class QuoteResponseDto
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public string MovementType { get; set; } = string.Empty;
    public string Incoterms { get; set; } = string.Empty;
    public string PackageType { get; set; } = string.Empty;
    public string OriginalCurrency { get; set; } = string.Empty;
    public decimal OriginalAmount { get; set; }
    public decimal UsdAmount { get; set; }
    public DateTime CreatedDate { get; set; }
}
