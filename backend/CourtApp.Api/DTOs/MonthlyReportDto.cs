namespace CourtApp.Api.DTOs;

public class MonthlyReportDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int TotalRequests { get; set; }
    public string? Department { get; set; } // Added to receive flat data from Dapper
    public int DepartmentCount { get; set; } // Added to receive flat data from Dapper
    public List<DepartmentBreakdownDto> DepartmentBreakdown { get; set; } = new List<DepartmentBreakdownDto>();
}

public class DepartmentBreakdownDto
{
    public string Department { get; set; } = string.Empty;
    public int Count { get; set; } // Changed from RequestCount to Count
}