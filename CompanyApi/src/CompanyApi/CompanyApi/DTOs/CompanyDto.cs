namespace CompanyApi.DTOs;

public record CompanyDto(
    int Id,
    string Name,
    string StockTicker,
    string Exchange,
    string Isin,
    string? Website,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateCompanyDto(
    string Name,
    string StockTicker,
    string Exchange,
    string Isin,
    string? Website
);

public record UpdateCompanyDto(
    string Name,
    string StockTicker,
    string Exchange,
    string Isin,
    string? Website
); 