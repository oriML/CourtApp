using Xunit;
using Moq;
using AutoMapper;
using CourtApp.Api.Interfaces.Services;
using CourtApp.Api.Interfaces.Repositories;
using CourtApp.Api.Models;
using CourtApp.Api.DTOs;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq; // Added for .Count()

namespace CourtApp.Api.Tests;

public class ContactRequestServiceTests
{
    private readonly Mock<IContactRequestRepository> _mockContactRequestRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ContactRequestService _service;

    public ContactRequestServiceTests()
    {
        _mockContactRequestRepository = new Mock<IContactRequestRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new ContactRequestService(_mockContactRequestRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateContactRequestAsync_ShouldReturnContactRequestDto_WhenSuccessful()
    {
        // Arrange
        var createDto = new CreateContactRequestDto { Email = "test@example.com", Name = "Test", Phone = "123", Description = "Desc", Departments = new[] { "Dept1" }, Status = "Open" };
        var contactRequest = new ContactRequest { Id = Guid.NewGuid(), Email = "test@example.com", Departments = new[] { "Dept1" }, Status = "Open" };
        var expectedDto = new ContactRequestDto { Id = contactRequest.Id, Email = "test@example.com", Departments = new[] { "Dept1" }, Status = "Open" };

        _mockContactRequestRepository.Setup(r => r.HasEmailBeenSentInTheLastMonthAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
        _mockMapper.Setup(m => m.Map<ContactRequest>(It.IsAny<CreateContactRequestDto>()))
            .Returns(contactRequest);
        _mockMapper.Setup(m => m.Map<ContactRequestDto>(It.IsAny<ContactRequest>()))
            .Returns(expectedDto);

        // Act
        var result = await _service.CreateContactRequestAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
        Assert.Equal(expectedDto.Status, result.Status); // Assert Status
        Assert.Contains("Dept1", result.Departments); // Assert Departments
        _mockContactRequestRepository.Verify(r => r.AddAsync(contactRequest, It.IsAny<CancellationToken>()), Times.Once);
        _mockContactRequestRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateContactRequestAsync_ShouldThrowException_WhenEmailSentRecently()
    {
        // Arrange
        var createDto = new CreateContactRequestDto { Email = "test@example.com" };

        _mockContactRequestRepository.Setup(r => r.HasEmailBeenSentInTheLastMonthAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CreateContactRequestAsync(createDto));
        _mockContactRequestRepository.Verify(r => r.AddAsync(It.IsAny<ContactRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockContactRequestRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteContactRequestAsync_ShouldDeleteContactRequest_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var contactRequest = new ContactRequest { Id = id };

        _mockContactRequestRepository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contactRequest);

        // Act
        await _service.DeleteContactRequestAsync(id);

        // Assert
        _mockContactRequestRepository.Verify(r => r.Delete(contactRequest), Times.Once);
        _mockContactRequestRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteContactRequestAsync_ShouldDoNothing_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mockContactRequestRepository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ContactRequest)null);

        // Act
        await _service.DeleteContactRequestAsync(id);

        // Assert
        _mockContactRequestRepository.Verify(r => r.Delete(It.IsAny<ContactRequest>()), Times.Never);
        _mockContactRequestRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetAllContactRequestsAsync_ShouldReturnListOfContactRequestDtos()
    {
        // Arrange
        var contactRequests = new List<ContactRequest> { new ContactRequest(), new ContactRequest() };
        var expectedDtos = new List<ContactRequestDto> { new ContactRequestDto(), new ContactRequestDto() };

        _mockContactRequestRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(contactRequests);
        _mockMapper.Setup(m => m.Map<IEnumerable<ContactRequestDto>>(It.IsAny<IEnumerable<ContactRequest>>()))
            .Returns(expectedDtos);

        // Act
        var result = await _service.GetAllContactRequestsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDtos.Count, result.Count());
    }

    [Fact]
    public async Task GetContactRequestByIdAsync_ShouldReturnContactRequestDto_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var contactRequest = new ContactRequest { Id = id };
        var expectedDto = new ContactRequestDto { Id = id };

        _mockContactRequestRepository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contactRequest);
        _mockMapper.Setup(m => m.Map<ContactRequestDto>(It.IsAny<ContactRequest>()))
            .Returns(expectedDto);

        // Act
        var result = await _service.GetContactRequestByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
    }

    [Fact]
    public async Task GetContactRequestByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mockContactRequestRepository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ContactRequest)null);

        // Act
        var result = await _service.GetContactRequestByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateContactRequestAsync_ShouldUpdateContactRequest_WhenSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = new UpdateContactRequestDto { Name = "Updated Name", Departments = new[] { "NewDept" }, Status = "Closed" };
        var contactRequest = new ContactRequest { Id = id, Name = "Original Name", Departments = new[] { "OldDept" }, Status = "Open" };

        _mockContactRequestRepository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contactRequest);
        _mockMapper.Setup(m => m.Map(updateDto, contactRequest))
            .Callback<UpdateContactRequestDto, ContactRequest>((src, dest) =>
            {
                dest.Name = src.Name;
                dest.Departments = src.Departments;
                dest.Status = src.Status;
            });

        // Act
        await _service.UpdateContactRequestAsync(id, updateDto);

        // Assert
        _mockContactRequestRepository.Verify(r => r.Update(contactRequest), Times.Once);
        _mockContactRequestRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal("Updated Name", contactRequest.Name); // Verify mapping happened
        Assert.Contains("NewDept", contactRequest.Departments); // Verify departments updated
        Assert.Equal("Closed", contactRequest.Status); // Verify status updated
    }

    [Fact]
    public async Task UpdateContactRequestAsync_ShouldThrowKeyNotFoundException_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = new UpdateContactRequestDto();

        _mockContactRequestRepository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ContactRequest)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateContactRequestAsync(id, updateDto));
        _mockContactRequestRepository.Verify(r => r.Update(It.IsAny<ContactRequest>()), Times.Never);
        _mockContactRequestRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateContactRequestAsync_ShouldThrowDbUpdateConcurrencyException_WhenConcurrencyIssue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = new UpdateContactRequestDto { Name = "Updated Name" };
        var contactRequest = new ContactRequest { Id = id, Name = "Original Name" };

        _mockContactRequestRepository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contactRequest);
        _mockMapper.Setup(m => m.Map(updateDto, contactRequest))
            .Callback<UpdateContactRequestDto, ContactRequest>((src, dest) =>
            {
                dest.Name = src.Name;
                dest.Departments = src.Departments; // Ensure departments are mapped
                dest.Status = src.Status; // Ensure status is mapped
            });
        _mockContactRequestRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateConcurrencyException());

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _service.UpdateContactRequestAsync(id, updateDto));
        _mockContactRequestRepository.Verify(r => r.Update(contactRequest), Times.Once);
        // Verify that GetByIdAsync is called again to refresh the entity's RowVersion
        _mockContactRequestRepository.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Exactly(2));
    }
}
