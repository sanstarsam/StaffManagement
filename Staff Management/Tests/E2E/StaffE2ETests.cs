using System.Net;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using Staff_Management.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Staff_Management.Tests.E2E
{
    public class StaffE2ETests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public StaffE2ETests(WebApplicationFactory<Program> factory)
        {
            // Create HTTP client for real API requests
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "POST /api/staffs should create a staff")]
        public async Task CreateStaff_ShouldReturnOk()
        {
            var staff = new StaffModel
            {
                StaffId = "E001",
                FullName = "E2E User",
                Birthday = new DateOnly(1990, 1, 1),
                Gender = 1
            };

            var response = await _client.PostAsJsonAsync("/api/staffs", staff);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var returned = await response.Content.ReadFromJsonAsync<StaffModel>();
            returned!.StaffId.Should().Be("E001");
        }

        [Fact(DisplayName = "GET /api/staffs should return staff list")]
        public async Task GetAllStaff_ShouldReturnList()
        {
            var response = await _client.GetAsync("/api/staffs");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<StaffModel>>();
            result.Should().NotBeNull();
        }

        [Fact(DisplayName = "PUT /api/staffs should update staff")]
        public async Task UpdateStaff_ShouldModifyStaff()
        {
            var updatedStaff = new StaffModel
            {
                StaffId = "E001",
                FullName = "Updated User",
                Birthday = new DateOnly(1991, 2, 2),
                Gender = 2
            };

            var response = await _client.PutAsJsonAsync("/api/staffs", updatedStaff);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("updated");
        }

        [Fact(DisplayName = "DELETE /api/staffs/{id} should delete staff")]
        public async Task DeleteStaff_ShouldReturnOk()
        {
            var response = await _client.DeleteAsync("/api/staffs/E001");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("deleted");
        }
    }
}
