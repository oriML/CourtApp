using Microsoft.AspNetCore.Mvc;
using CourtApp.Api.Data;
using CourtApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourtApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public SeedController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpPost("init")]
    public async Task<IActionResult> Init()
    {
        if (!_environment.IsDevelopment())
        {
            return Forbid(); // Only allow in Development environment
        }

        // Ensure the database is migrated
        await _context.Database.MigrateAsync();

        if (await _context.ContactRequests.AnyAsync())
        {
            return Ok(new { success = true, seeded = 0, message = "Database already seeded." });
        }

        var contactRequests = new List<ContactRequest>
        {
            new ContactRequest
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Phone = "111-222-3333",
                Email = "john.doe@example.com",
                Departments = new[] { "Sales", "Support" },
                Description = "Inquiry about product features.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = "Open"
            },
            new ContactRequest
            {
                Id = Guid.NewGuid(),
                Name = "Jane Smith",
                Phone = "444-555-6666",
                Email = "jane.smith@example.com",
                Departments = new[] { "IT" },
                Description = "Request for technical assistance.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = "InProgress"
            },
            new ContactRequest
            {
                Id = Guid.NewGuid(),
                Name = "Peter Jones",
                Phone = "777-888-9999",
                Email = "peter.jones@example.com",
                Departments = new[] { "Sales" },
                Description = "Interested in a demo.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = "Closed"
            }
        };

        await _context.ContactRequests.AddRangeAsync(contactRequests);
        var seededCount = await _context.SaveChangesAsync();

        return Ok(new { success = true, seeded = seededCount, message = $"{seededCount} records inserted." });
    }
}
