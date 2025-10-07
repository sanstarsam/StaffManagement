using Microsoft.AspNetCore.Mvc;
using Moq;
using Staff_Management.Controllers;
using Staff_Management.Models;
using Staff_Management.Repositories;
using Xunit;
using System.Threading.Tasks;

namespace Staff_Management.Tests.Unit
{
    public class StaffControllerTests
    {
        private readonly Mock<StaffRepository> _mockRepo;
        private readonly StaffsController _controller;

        public StaffControllerTests()
        {
            _mockRepo = new Mock<StaffRepository>();
            _controller = new StaffsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithStaffList()
        {
            var staffs = new List<StaffModel>
            {
                new() { StaffId = "S001", FullName = "John Doe", Gender = 1, Birthday = new DateOnly(1990,1,1) },
                new() { StaffId = "S002", FullName = "Jane Doe", Gender = 2, Birthday = new DateOnly(1995,1,1) }
            };

            _mockRepo.Setup(r => r.GetAll()).Returns(staffs);

            var filter = new StaffFilter();

            var result = await _controller.GetAll(filter) as OkObjectResult;

            Assert.NotNull(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<StaffModel>>(result.Value);
            Assert.Equal(2, returned.Count());
        }

        [Fact]
        public async Task GetStaff_ReturnsOk_WhenFound()
        {
            var staff = new StaffModel { StaffId = "S001", FullName = "John Doe" };
            _mockRepo.Setup(r => r.GetById("S001")).Returns(staff);

            var result = await _controller.GetStaff("S001") as OkObjectResult;

            Assert.NotNull(result);
            var returned = Assert.IsType<StaffModel>(result.Value);
            Assert.Equal("S001", returned.StaffId);
        }

        [Fact]
        public async Task GetStaff_ReturnsNotFound_WhenMissing()
        {
            _mockRepo.Setup(r => r.GetById("X999")).Returns((StaffModel)null);

            var result = await _controller.GetStaff("X999");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsOk_AndAddsStaff()
        {
            var newStaff = new StaffModel { StaffId = "S003", FullName = "Lisa" };

            var result = await _controller.Create(newStaff) as OkObjectResult;

            _mockRepo.Verify(r => r.Add(newStaff), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(newStaff, result.Value);
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenSuccess()
        {
            var staff = new StaffModel { StaffId = "S001", FullName = "John Updated" };
            _mockRepo.Setup(r => r.Update("S001", staff)).Returns(true);

            var result = await _controller.Update(staff) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Staff updated", result.Value);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenFail()
        {
            var staff = new StaffModel { StaffId = "S999" };
            _mockRepo.Setup(r => r.Update("S999", staff)).Returns(false);

            var result = await _controller.Update(staff);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenDeleted()
        {
            _mockRepo.Setup(r => r.Delete("S001")).Returns(true);

            var result = await _controller.Delete("S001") as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Staff deleted", result.Value);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenNotFound()
        {
            _mockRepo.Setup(r => r.Delete("S002")).Returns(false);

            var result = await _controller.Delete("S002");

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
