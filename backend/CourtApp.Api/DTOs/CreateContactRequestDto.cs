namespace CourtApp.Api.DTOs;

public class CreateContactRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string[] Departments { get; set; } = Array.Empty<string>();
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Open"; // Added Status with default value
}