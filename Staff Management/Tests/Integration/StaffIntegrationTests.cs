using Microsoft.AspNetCore.Mvc;
using Staff_Management.Classes;
using Staff_Management.Controllers;
using Staff_Management.Models;
using Staff_Management.Repositories;
using Xunit;

namespace Staff_Management.Tests.Integration
{
    public class StaffIntegrationTests
    {
        private readonly string _testFilePath;
        private readonly StaffRepository _repository;
        private readonly StaffsController _controller;

        public StaffIntegrationTests()
        {
            _testFilePath = Path.Combine(Path.GetTempPath(), $"staff_integration_{Guid.NewGuid()}.xml");

            var store = new XmlDataStore<StaffModel>(_testFilePath, "Staffs");
            _repository = new StaffRepository(store);
            _controller = new StaffsController(_repository);
        }

        [Fact]
        public async Task Create_And_GetStaff_ShouldWork_EndToEnd()
        {
            var newStaff = new StaffModel
            {
                StaffId = "S100",
                FullName = "Integration User",
                Birthday = new DateOnly(2000, 1, 1),
                Gender = 1
            };

            var createResult = await _controller.Create(newStaff) as OkObjectResult;
            Assert.NotNull(createResult);
            Assert.Equal(newStaff, createResult.Value);

            var getResult = await _controller.GetStaff("S100") as OkObjectResult;
            Assert.NotNull(getResult);

            var staff = getResult.Value as StaffModel;

            Assert.Equal("Integration User", staff!.FullName);
            Assert.Equal(1, staff.Gender);
        }

        [Fact]
        public async Task Update_And_Delete_ShouldPersistInXml()
        {
            var staff = new StaffModel
            {
                StaffId = "S200",
                FullName = "Original Name",
                Birthday = new DateOnly(1995, 2, 2),
                Gender = 0
            };
            await _controller.Create(staff);

            staff.FullName = "Updated Name";
            var updateResult = await _controller.Update(staff) as OkObjectResult;
            Assert.NotNull(updateResult);
            Assert.Equal("Staff updated", updateResult.Value);

            var reloaded = _repository.GetById("S200");
            Assert.NotNull(reloaded);
            Assert.Equal("Updated Name", reloaded!.FullName);

            var deleteResult = await _controller.Delete("S200") as OkObjectResult;
            Assert.NotNull(deleteResult);
            Assert.Equal("Staff deleted", deleteResult.Value);

            var deleted = _repository.GetById("S200");
            Assert.Null(deleted);
        }

        ~StaffIntegrationTests()
        {
            // Cleanup temp file after test
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);
        }
    }
}
