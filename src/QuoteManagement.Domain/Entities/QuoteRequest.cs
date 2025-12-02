namespace QuoteManagement.Domain.Entities;

public class QuoteRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    // General Fields
    public string Mode { get; set; } = string.Empty; // LCL, FCL, Air
    public string MovementType { get; set; } = string.Empty; // Door to Door, Port to Door, etc.
    public string Incoterms { get; set; } = string.Empty; // DDP, DAT
    public string PackageType { get; set; } = string.Empty; // Pallets, Boxes, Cartons
    
    // Location
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    
    // Currency
    public string OriginalCurrency { get; set; } = string.Empty; // USD, CNY, TRY
    public decimal OriginalAmount { get; set; }
    public decimal UsdAmount { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    // Navigation property
    public User User { get; set; } = null!;
}
