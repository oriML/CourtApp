using AutoMapper;
using CourtApp.Api.DTOs;
using CourtApp.Api.Models;
using CourtApp.Api.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Add this using statement

using CourtApp.Api.Interfaces.Services;

public class ContactRequestService : IContactRequestService
{
    private readonly IContactRequestRepository _contactRequestRepository;
    private readonly IMapper _mapper;

    public ContactRequestService(IContactRequestRepository contactRequestRepository, IMapper mapper)
    {
        _contactRequestRepository = contactRequestRepository;
        _mapper = mapper;
    }

    public async Task<ContactRequestDto> CreateContactRequestAsync(CreateContactRequestDto createContactRequestDto, CancellationToken cancellationToken = default)
    {
        if (await _contactRequestRepository.HasEmailBeenSentInTheLastMonthAsync(createContactRequestDto.Email))
        {
            throw new Exception("An email has already been sent from this address in the last month.");
        }

        var contactRequest = _mapper.Map<ContactRequest>(createContactRequestDto);
        contactRequest.Id = Guid.NewGuid();
        contactRequest.Phone = NormalizePhoneNumber(contactRequest.Phone);
        await _contactRequestRepository.AddAsync(contactRequest, cancellationToken);
        await _contactRequestRepository.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ContactRequestDto>(contactRequest);
    }

    public async Task DeleteContactRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contactRequest = await _contactRequestRepository.GetByIdAsync(id, cancellationToken);
        if (contactRequest != null)
        {
            _contactRequestRepository.Delete(contactRequest);
            await _contactRequestRepository.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<ContactRequestDto>> GetAllContactRequestsAsync(CancellationToken cancellationToken = default)
    {
        var contactRequests = await _contactRequestRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ContactRequestDto>>(contactRequests);
    }

    public async Task<ContactRequestDto> GetContactRequestByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contactRequest = await _contactRequestRepository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<ContactRequestDto>(contactRequest);
    }

    public async Task<IEnumerable<MonthlyReportDto>> GetMonthlyReportAsync(CancellationToken cancellationToken = default)
    {
        return await _contactRequestRepository.GetMonthlyReportAsync(cancellationToken);
    }

    public async Task UpdateContactRequestAsync(Guid id, UpdateContactRequestDto updateContactRequestDto, CancellationToken cancellationToken = default)
    {
        var contactRequest = await _contactRequestRepository.GetByIdAsync(id, cancellationToken);
        if (contactRequest == null)
        {
            throw new KeyNotFoundException($"ContactRequest with ID {id} not found.");
        }

        _mapper.Map(updateContactRequestDto, contactRequest);
        contactRequest.Phone = NormalizePhoneNumber(contactRequest.Phone);

        try
        {
            _contactRequestRepository.Update(contactRequest);
            await _contactRequestRepository.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            // Reload the entity from the database to get the latest RowVersion
            await _contactRequestRepository.GetByIdAsync(id, cancellationToken); // This will refresh the entity's RowVersion
            throw; // Re-throw the exception to be handled by the controller
        }
    }

    private string NormalizePhoneNumber(string phoneNumber)
    {
        return Regex.Replace(phoneNumber, @"[^\d]", "");
    }
}