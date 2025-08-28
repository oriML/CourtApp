using CourtApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CourtApp.Api.DTOs;

namespace CourtApp.Api.Interfaces.Repositories;

public interface IContactRequestRepository
{
    Task<ContactRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ContactRequest>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ContactRequest contactRequest, CancellationToken cancellationToken = default);
    void Update(ContactRequest contactRequest);
    void Delete(ContactRequest contactRequest);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> HasEmailBeenSentInTheLastMonthAsync(string email);
    Task<IEnumerable<MonthlyReportDto>> GetMonthlyReportAsync(CancellationToken cancellationToken = default);
}