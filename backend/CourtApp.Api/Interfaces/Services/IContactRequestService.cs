using CourtApp.Api.DTOs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CourtApp.Api.Interfaces.Services;

public interface IContactRequestService
{
    Task<ContactRequestDto> GetContactRequestByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ContactRequestDto>> GetAllContactRequestsAsync(CancellationToken cancellationToken = default);
    Task<ContactRequestDto> CreateContactRequestAsync(CreateContactRequestDto createContactRequestDto, CancellationToken cancellationToken = default);
    /// <summary>
    /// Updates a contact request.
    /// </summary>
    /// <param name="id">The ID of the contact request to update.</param>
    /// <param name="updateContactRequestDto">The DTO containing updated contact request data.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="System.Collections.Generic.KeyNotFoundException">Thrown when the contact request with the specified ID is not found.</exception>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">Thrown when a concurrency conflict occurs during the update operation.</exception>
    Task UpdateContactRequestAsync(Guid id, UpdateContactRequestDto updateContactRequestDto, CancellationToken cancellationToken = default);
    Task DeleteContactRequestAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MonthlyReportDto>> GetMonthlyReportAsync(CancellationToken cancellationToken = default);
}