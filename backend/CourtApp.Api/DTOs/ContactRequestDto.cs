using System;

namespace CourtApp.Api.DTOs;

public class ContactRequestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string[] Departments { get; set; } = Array.Empty<string>();
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = string.Empty; // Added Status
}