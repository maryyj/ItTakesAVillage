using ItTakesAVillage.Contracts;
using ItTakesAVillage.Data;
using ItTakesAVillage.Models;
using ItTakesAVillage.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace ItTakesAVillage.Tests.UnitTests
{
    public class GroupTests
    {
        private readonly Mock<IRepository<UserGroup>> _userGroupRepositoryMock;
        private readonly Mock<IRepository<ItTakesAVillageUser>> _userRepositoryMock;
        private readonly Mock<IRepository<Group>> _groupRepositoryMock;
        private readonly GroupService _sut;
        public GroupTests()
        {
            _userGroupRepositoryMock = new Mock<IRepository<UserGroup>>();
            _userRepositoryMock = new Mock<IRepository<ItTakesAVillageUser>>();
            _groupRepositoryMock = new Mock<IRepository<Group>>();

            _sut = new GroupService(_groupRepositoryMock.Object,
                                   _userRepositoryMock.Object,
                                   _userGroupRepositoryMock.Object);
        }

        [Theory]
        [InlineData("TestGroup")]
        [InlineData("testgroup")]
        [InlineData("testgroup!")]
        [InlineData("&testgroup")]
        [InlineData("1testgroup")]
        [InlineData("testgroup0")]
        [InlineData(" Testgroup")]
        public async Task Save_WhenGroupDoesNotExist_ReturnsGroupId(string expected)
        {
            // Arrange
            var group = new Group { Id = 1, Name = expected };
            var userId = "testUserId";

            _userGroupRepositoryMock.Setup(x => x.GetByFilterAsync(It.IsAny<Expression<Func<UserGroup, bool>>>()))
                                  .ReturnsAsync(new List<UserGroup>());

            _groupRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Group>()))
                              .Callback((Group g) => { g.Id = group.Id; })
                              .Returns(Task.CompletedTask);
            // Act
            var actual = await _sut.Save(group, userId);

            // Assert
            Assert.Equal(group.Id, actual);
            _groupRepositoryMock.Verify(x => x.AddAsync(It.Is<Group>(g => g.Equals(group))), Times.Once);
        }

        [Theory]
        [InlineData("TestGroup")]
        [InlineData("testgroup")]
        [InlineData("testgroup!")]
        [InlineData("&testgroup")]
        [InlineData("1testgroup")]
        [InlineData("testgroup0")]
        [InlineData(" Testgroup")]
        public async Task Save_WhenGroupExists_ReturnsZero(string expected)
        {
            // Arrange
            var existingGroup = new Group { Id = 1, Name = expected };
            var group = new Group { Id = 2, Name = expected };
            var userId = "testUserId";

            _userGroupRepositoryMock.Setup(x => x.GetByFilterAsync(It.IsAny<Expression<Func<UserGroup, bool>>>()))
                                  .ReturnsAsync(new List<UserGroup> { new UserGroup { UserId = userId, Group = existingGroup } });
            // Act
            var actual = await _sut.Save(group, userId);

            // Assert
            Assert.Equal(0, actual);

            _groupRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Group>()), Times.Never);
        }

        [Fact]
        public async Task AddUser_UserNotInList_ShouldAddUserAndReturnTrue()
        {
            // Arrange
            var userId = "expectedUserId";
            var groupId = 1;
            var userGroups = new List<UserGroup>();

            _userRepositoryMock.Setup(x => x.GetAsync(userId)).ReturnsAsync(new ItTakesAVillageUser { Id = userId });
            _userGroupRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(userGroups);

            // Act
            var actual = await _sut.AddUser(userId, groupId);

            // Assert
            Assert.True(actual);
            _userGroupRepositoryMock.Verify(x => x.AddAsync(It.Is<UserGroup>(ug => ug.UserId == userId && ug.GroupId == groupId)), Times.Once);
        }

        [Fact]
        public async Task AddUser_UserAlreadyInList_ShouldNotAddUserAndReturnFalse()
        {
            // Arrange
            var expectedUserId = "expectedUserId";
            var expectedGroupId = 1;
            var userGroups = new List<UserGroup>
            {
                new UserGroup { UserId = expectedUserId, GroupId = expectedGroupId }
            };

            _userRepositoryMock.Setup(x => x.GetAsync(expectedUserId)).ReturnsAsync(new ItTakesAVillageUser { Id = expectedUserId });

            _userGroupRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(userGroups);

            // Act
            var actual = await _sut.AddUser(expectedUserId, expectedGroupId);

            // Assert
            Assert.False(actual);
        }
    }
}