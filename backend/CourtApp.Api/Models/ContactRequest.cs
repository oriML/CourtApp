using System;
using System.ComponentModel.DataAnnotations;

namespace CourtApp.Api.Models;

public class ContactRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string[] Departments { get; set; } = Array.Empty<string>(); // Changed back to string array
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } // Added UpdatedAt
    public uint RowVersion { get; set; } // Changed from byte[] to uint
    public string Status { get; set; } = "Open";
}