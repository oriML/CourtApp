using CourtApp.Api.DTOs;
using CourtApp.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic; // Add this using statement
using Microsoft.EntityFrameworkCore; // Add this using statement

namespace CourtApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Add this attribute to secure all endpoints in this controller
public class ContactRequestsController : ControllerBase
{
    private readonly IContactRequestService _contactRequestService;

    public ContactRequestsController(IContactRequestService contactRequestService)
    {
        _contactRequestService = contactRequestService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var contactRequests = await _contactRequestService.GetAllContactRequestsAsync(cancellationToken);
        return Ok(contactRequests);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var contactRequest = await _contactRequestService.GetContactRequestByIdAsync(id, cancellationToken);
        if (contactRequest == null)
        {
            return NotFound();
        }
        return Ok(contactRequest);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateContactRequestDto createContactRequestDto, CancellationToken cancellationToken)
    {
        var createdContactRequest = await _contactRequestService.CreateContactRequestAsync(createContactRequestDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = createdContactRequest.Id }, createdContactRequest);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateContactRequestDto updateContactRequestDto, CancellationToken cancellationToken)
    {
        try
        {
            await _contactRequestService.UpdateContactRequestAsync(id, updateContactRequestDto, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict("Concurrency conflict: The record was modified by another user.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _contactRequestService.DeleteContactRequestAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("report/monthly")]
    public async Task<IActionResult> GetMonthlyReport(CancellationToken cancellationToken)
    {
        var report = await _contactRequestService.GetMonthlyReportAsync(cancellationToken);
        return Ok(report);
    }
}