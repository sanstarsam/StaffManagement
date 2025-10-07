using Moq;
using Staff_Management.Classes;
using Staff_Management.Models;
using Staff_Management.Repositories;
using System.Xml.Linq;
using Xunit;

namespace Staff_Management.Tests.Unit
{
    public class StaffRepositoryTests
    {
        private readonly Mock<XmlDataStore<StaffModel>> _mockStore;
        private readonly StaffRepository _repo;
        private readonly List<StaffModel> _fakeData;

        public StaffRepositoryTests() 
        {
            _mockStore = new Mock<XmlDataStore<StaffModel>>();
            _repo = new StaffRepository(_mockStore.Object);

            _fakeData = new List<StaffModel>
            {
                new() { StaffId = "S001", FullName = "John Doe", Birthday = new DateOnly(1990,1,1), Gender = 1 },
                new() { StaffId = "S002", FullName = "Jane Smith", Birthday = new DateOnly(1995,5,5), Gender = 2 }
            };
        }

        [Fact]
        public void GetAll_ShouldReturnList()
        {
            _mockStore.Setup(s => s.LoadAll(It.IsAny<Func<XElement, StaffModel>>())).Returns(_fakeData);

            var result = _repo.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("S001", result.First().StaffId);
        }

        [Fact]
        public void GetById_ShouldReturnStaff_WhenExists()
        {
            _mockStore.Setup(s => s.LoadAll(It.IsAny<Func<XElement, StaffModel>>())).Returns(_fakeData);

            var staff = _repo.GetById("S002");

            Assert.NotNull(staff);
            Assert.Equal("Jane Smith", staff!.FullName);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenNotFound()
        {
            _mockStore.Setup(s => s.LoadAll(It.IsAny<Func<XElement, StaffModel>>())).Returns(_fakeData);

            var staff = _repo.GetById("X999");

            Assert.Null(staff);
        }

        [Fact]
        public void Add_ShouldCallAppend()
        {
            var newStaff = new StaffModel { StaffId = "S003", FullName = "Alex" };

            _repo.Add(newStaff);

            _mockStore.Verify(s => s.Append(newStaff, It.IsAny<Func<StaffModel, XElement>>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturnTrue_WhenStaffExists()
        {
            _mockStore.Setup(s => s.LoadAll(It.IsAny<Func<XElement, StaffModel>>())).Returns(_fakeData);

            var updated = new StaffModel { StaffId = "S001", FullName = "Updated", Gender = 1, Birthday = new DateOnly(2000, 1, 1) };

            var result = _repo.Update("S001", updated);

            Assert.True(result);
            _mockStore.Verify(s => s.SaveAll(It.IsAny<IEnumerable<StaffModel>>(), It.IsAny<Func<StaffModel, XElement>>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturnFalse_WhenNotFound()
        {
            _mockStore.Setup(s => s.LoadAll(It.IsAny<Func<XElement, StaffModel>>())).Returns(_fakeData);

            var updated = new StaffModel { StaffId = "S999", FullName = "Ghost" };

            var result = _repo.Update("S999", updated);

            Assert.False(result);
            _mockStore.Verify(s => s.SaveAll(It.IsAny<IEnumerable<StaffModel>>(), It.IsAny<Func<StaffModel, XElement>>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldRemoveAndSave_WhenExists()
        {
            _mockStore.Setup(s => s.LoadAll(It.IsAny<Func<XElement, StaffModel>>())).Returns(_fakeData);

            var result = _repo.Delete("S001");

            Assert.True(result);
            _mockStore.Verify(s => s.SaveAll(It.IsAny<IEnumerable<StaffModel>>(), It.IsAny<Func<StaffModel, XElement>>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldReturnFalse_WhenNotFound()
        {
            _mockStore.Setup(s => s.LoadAll(It.IsAny<Func<XElement, StaffModel>>())).Returns(_fakeData);

            var result = _repo.Delete("S999");

            Assert.False(result);
            _mockStore.Verify(s => s.SaveAll(It.IsAny<IEnumerable<StaffModel>>(), It.IsAny<Func<StaffModel, XElement>>()), Times.Never);
        }
    }
}
