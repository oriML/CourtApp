using CourtApp.Api.Data;
using CourtApp.Api.DTOs;
using CourtApp.Api.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

using CourtApp.Api.Interfaces.Repositories;

public class ContactRequestRepository : IContactRequestRepository
{
    private readonly AppDbContext _context;
    private readonly string _connectionString;
    private readonly ILogger<ContactRequestRepository> _logger;

    public ContactRequestRepository(AppDbContext context, IConfiguration configuration, ILogger<ContactRequestRepository> logger)
    {
        _context = context;
        _logger = logger;
        _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                            throw new InvalidOperationException("Connection string is not configured.");
    }

    public async Task AddAsync(ContactRequest contactRequest, CancellationToken cancellationToken = default)
    {
        await _context.ContactRequests.AddAsync(contactRequest, cancellationToken);
    }

    public async Task<IEnumerable<ContactRequest>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ContactRequests.ToListAsync(cancellationToken);
    }

    public async Task<ContactRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ContactRequests.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
    }

    public void Update(ContactRequest contactRequest)
    {
        _context.ContactRequests.Update(contactRequest);
    }

    public void Delete(ContactRequest contactRequest)
    {
        _context.ContactRequests.Remove(contactRequest);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<MonthlyReportDto>> GetMonthlyReportAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        try
        {
            const string checkProcedureSql = "SELECT 1 FROM pg_proc WHERE proname = 'sp_get_monthly_contact_report'";
            var procedureExists = await connection.ExecuteScalarAsync<int>(checkProcedureSql) == 1;

            if (!procedureExists)
            {
                var createProcedureSql = await File.ReadAllTextAsync("sp_GetMonthlyContactReport.sql", cancellationToken);
                await connection.ExecuteAsync(createProcedureSql);
            }

            var report = await connection.QueryAsync<MonthlyReportDto>(
                "sp_get_monthly_contact_report",
                commandType: CommandType.StoredProcedure
            );

            return report;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the monthly report.");
            throw;
        }
    }


    public async Task<bool> HasEmailBeenSentInTheLastMonthAsync(string email)
    {
        var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
        return await _context.ContactRequests.AnyAsync(cr => cr.Email == email && cr.CreatedAt > oneMonthAgo);
    }
}