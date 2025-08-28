using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using CourtApp.Api.DTOs;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq; // Added for .Contains()

namespace CourtApp.Api.Tests;

public class ContactRequestsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ContactRequestsControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    private async Task<string> GetAdminToken()
    {
        var loginDto = new LoginRequestDto { Username = "admin", Password = "password" };
        var json = JsonConvert.SerializeObject(loginDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/Auth/login", content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(responseString);
        return loginResponse.Token;
    }

    [Fact]
    public async Task GetAll_ReturnsSuccessStatusCode()
    {
        // Arrange
        var token = await GetAdminToken();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // Act
        var response = await _client.GetAsync("/ContactRequests");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateContactRequest_ReturnsCreatedStatusCode()
    {
        // Arrange
        var token = await GetAdminToken();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var createDto = new CreateContactRequestDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Phone = "1234567890",
            Description = "Test description",
            Departments = new[] { "Sales" }
        };
        var json = JsonConvert.SerializeObject(createDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/ContactRequests", content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var contactRequest = JsonConvert.DeserializeObject<ContactRequestDto>(responseString);
        Assert.NotNull(contactRequest);
        Assert.NotEqual(Guid.Empty, contactRequest.Id);
        Assert.Equal("Open", contactRequest.Status); // Assert default status
        Assert.Contains("Sales", contactRequest.Departments); // Assert departments
    }

    [Fact]
    public async Task UpdateContactRequest_ReturnsNoContentStatusCode()
    {
        // Arrange
        var token = await GetAdminToken();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // First, create a contact request to update
        var createDto = new CreateContactRequestDto
        {
            Name = "Original Name",
            Email = "update@example.com",
            Phone = "1112223333",
            Description = "Original description",
            Departments = new[] { "Support" }
        };
        var createJson = JsonConvert.SerializeObject(createDto);
        var createContent = new StringContent(createJson, Encoding.UTF8, "application/json");
        var createResponse = await _client.PostAsync("/ContactRequests", createContent);
        createResponse.EnsureSuccessStatusCode();
        var createdContact = JsonConvert.DeserializeObject<ContactRequestDto>(await createResponse.Content.ReadAsStringAsync());

        var updateDto = new UpdateContactRequestDto
        {
            Name = "Updated Name",
            Email = createdContact.Email,
            Phone = createdContact.Phone,
            Description = createdContact.Description,
            Departments = new[] { "IT", "Support" }, // Updated departments
            Status = "InProgress" // Updated status
        };
        var updateJson = JsonConvert.SerializeObject(updateDto);
        var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/ContactRequests/{createdContact.Id}", updateContent);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify the update
        var getResponse = await _client.GetAsync($"/ContactRequests/{createdContact.Id}");
        getResponse.EnsureSuccessStatusCode();
        var updatedContact = JsonConvert.DeserializeObject<ContactRequestDto>(await getResponse.Content.ReadAsStringAsync());
        Assert.Equal("Updated Name", updatedContact.Name);
        Assert.Equal("InProgress", updatedContact.Status);
        Assert.Contains("IT", updatedContact.Departments);
        Assert.Contains("Support", updatedContact.Departments);
    }

    [Fact]
    public async Task UpdateContactRequest_ReturnsConflictStatusCode_OnConcurrencyIssue()
    {
        // Arrange
        var token = await GetAdminToken();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // First, create a contact request
        var createDto = new CreateContactRequestDto
        {
            Name = "Concurrency Test",
            Email = "concurrency@example.com",
            Phone = "9998887777",
            Description = "Concurrency description",
            Departments = new[] { "HR" }
        };
        var createJson = JsonConvert.SerializeObject(createDto);
        var createContent = new StringContent(createJson, Encoding.UTF8, "application/json");
        var createResponse = await _client.PostAsync("/ContactRequests", createContent);
        createResponse.EnsureSuccessStatusCode();
        var createdContact = JsonConvert.DeserializeObject<ContactRequestDto>(await createResponse.Content.ReadAsStringAsync());

        // Simulate a concurrent update by fetching the entity, modifying it, and saving it
        // This will update the RowVersion in the database
        var firstUpdateDto = new UpdateContactRequestDto
        {
            Name = "First Update",
            Email = createdContact.Email,
            Phone = createdContact.Phone,
            Description = createdContact.Description,
            Departments = createdContact.Departments,
            Status = "InProgress"
        };
        var firstUpdateJson = JsonConvert.SerializeObject(firstUpdateDto);
        var firstUpdateContent = new StringContent(firstUpdateJson, Encoding.UTF8, "application/json");
        var firstUpdateResponse = await _client.PutAsync($"/ContactRequests/{createdContact.Id}", firstUpdateContent);
        firstUpdateResponse.EnsureSuccessStatusCode();

        // Now, try to update with the original RowVersion (from createdContact)
        // To simulate concurrency, we need to get the original RowVersion from the createdContact
        // and pass it in the updateDto. However, ContactRequestDto does not expose RowVersion.
        // For integration tests with WebApplicationFactory, simulating true concurrency
        // with RowVersion requires more advanced setup (e.g., custom DbContext in factory
        // that exposes RowVersion for testing purposes, or direct DB access in test).
        // For now, this test will check if the endpoint returns Conflict if the underlying
        // EF Core detects a concurrency issue. The actual trigger might be different
        // without explicitly passing the RowVersion in the DTO.
        var secondUpdateDto = new UpdateContactRequestDto
        {
            Name = "Second Update",
            Email = createdContact.Email,
            Phone = createdContact.Phone,
            Description = createdContact.Description,
            Departments = createdContact.Departments,
            Status = "Closed"
        };
        var secondUpdateJson = JsonConvert.SerializeObject(secondUpdateDto);
        var secondUpdateContent = new StringContent(secondUpdateJson, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/ContactRequests/{createdContact.Id}", secondUpdateContent);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task DeleteContactRequest_ReturnsNoContentStatusCode()
    {
        // Arrange
        var token = await GetAdminToken();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // First, create a contact request to delete
        var createDto = new CreateContactRequestDto
        {
            Name = "Delete User",
            Email = "delete@example.com",
            Phone = "9876543210",
            Description = "Delete description",
            Departments = new[] { "Marketing" }
        };
        var createJson = JsonConvert.SerializeObject(createDto);
        var createContent = new StringContent(createJson, Encoding.UTF8, "application/json");
        var createResponse = await _client.PostAsync("/ContactRequests", createContent);
        createResponse.EnsureSuccessStatusCode();
        var createdContact = JsonConvert.DeserializeObject<ContactRequestDto>(await createResponse.Content.ReadAsStringAsync());

        // Act
        var response = await _client.DeleteAsync($"/ContactRequests/{createdContact.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify it's actually deleted
        var getResponse = await _client.GetAsync($"/ContactRequests/{createdContact.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenContactRequestDoesNotExist()
    {
        // Arrange
        var token = await GetAdminToken();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/ContactRequests/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}