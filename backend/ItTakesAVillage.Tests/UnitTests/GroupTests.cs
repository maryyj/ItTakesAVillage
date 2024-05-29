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
        #region Create
        [Theory]
        [InlineData("TestGroup")]
        [InlineData("testgroup")]
        [InlineData("testgroup!")]
        [InlineData("&testgroup")]
        [InlineData("1testgroup")]
        [InlineData("testgroup0")]
        [InlineData(" Testgroup")]
        public async Task Save_WhenGroupDoesNotExist_ReturnsTrue(string expected)
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
            var actual = await _sut.CreateAsync(group, userId);

            // Assert
            Assert.True(actual);
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
        public async Task Save_WhenGroupExists_ReturnsFalse(string expected)
        {
            // Arrange
            var existingGroup = new Group { Id = 1, Name = expected };
            var group = new Group { Id = 2, Name = expected };
            var userId = "testUserId";

            _userGroupRepositoryMock.Setup(x => x.GetByFilterAsync(It.IsAny<Expression<Func<UserGroup, bool>>>()))
                                  .ReturnsAsync(new List<UserGroup> { new UserGroup { UserId = userId, Group = existingGroup } });
            // Act
            var actual = await _sut.CreateAsync(group, userId);

            // Assert
            Assert.False(actual);

            _groupRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Group>()), Times.Never);
        }
        #endregion
        #region Add
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
            var actual = await _sut.AddUserAsync(userId, groupId);

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
            var actual = await _sut.AddUserAsync(expectedUserId, expectedGroupId);

            // Assert
            Assert.False(actual);
        }
        #endregion
        #region DeleteUser
        [Fact]
        public async Task DeleteUser_UserGroupIsNull_ShouldNotDeleteUserAndReturnFalse()
        {
            string userId = "user1";
            int groupId = 1;

            //Arrange
            _userGroupRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync((List<UserGroup>)null);

            //Act
            var result = await _sut.DeleteUserAsync(userId, groupId);

            //Assert
            Assert.False(result);
            _userGroupRepositoryMock.Verify(x => x.DeleteAsync(It.Is<UserGroup>(x => x.UserId == userId && x.GroupId == groupId)), Times.Never);
        }
        [Fact]
        public async Task DeleteUser_UserGroupNotFound_ShouldNotDeleteUserAndReturnFalse()
        {
            // Arrange
            string userId = "user1";
            int groupId = 1;
            var userGroups = new List<UserGroup>
             {
                new() { UserId = "user2", GroupId = 1 },
                new() { UserId = "user3", GroupId = 2 }
             };
            _userGroupRepositoryMock.Setup(x => x.GetAsync())
                                    .ReturnsAsync(userGroups);
            // Act
            var result = await _sut.DeleteUserAsync(userId, groupId);

            // Assert
            Assert.False(result);
            _userGroupRepositoryMock.Verify(x => x.DeleteAsync(It.Is<UserGroup>(x => x.UserId == userId && x.GroupId == groupId)), Times.Never);

        }
        [Fact]
        public async Task DeleteUser_UserGroupFound_ShouldDeletesUserAndReturnTrue()
        {
            // Arrange
            string userId = "user1";
            int groupId = 1;
            var userGroups = new List<UserGroup>
            {
                new() { UserId = "user1", GroupId = 1 },
                new() { UserId = "user2", GroupId = 2 }
            };
            _userGroupRepositoryMock.Setup(x => x.GetAsync())
                                    .ReturnsAsync(userGroups);

            _userGroupRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<UserGroup>()))
                                    .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.DeleteUserAsync(userId, groupId);

            // Assert
            Assert.True(result);
            _userGroupRepositoryMock.Verify(x => x.DeleteAsync(It.Is<UserGroup>(x => x.UserId == userId && x.GroupId == groupId)), Times.Once);
        }
        #endregion

        #region DeleteGroup
        [Fact]
        public async Task DeleteGroup_UserGroupsIsNull_ShouldNotDeleteGroupAndReturnFalse()
        {
            // Arrange
            string userId = "user1";
            int groupId = 1;
            _userGroupRepositoryMock.Setup(x => x.GetAsync())
                                    .ReturnsAsync((List<UserGroup>)null);

            // Act
            var result = await _sut.DeleteAsync(userId, groupId);

            // Assert
            Assert.False(result);
            _userGroupRepositoryMock.Verify(x => x.DeleteAsync(It.Is<UserGroup>(x => x.UserId == userId && x.GroupId == groupId)), Times.Never);
        }
        [Fact]
        public async Task DeleteGroup_MoreThanOneUserInGroup_ShouldNotDeleteGroupAndReturnFalse()
        {
            string userId = "user1";
            int groupId = 1;
            // Arrange
            var userGroups = new List<UserGroup>
             {
                new() { UserId = "user1", GroupId = 1 },
                new() { UserId = "user2", GroupId = 1 }
             };
            _userGroupRepositoryMock.Setup(x => x.GetAsync())
                                    .ReturnsAsync(userGroups);

            // Act
            var result = await _sut.DeleteAsync(userId, groupId);

            // Assert
            Assert.False(result);
            _userGroupRepositoryMock.Verify(x => x.DeleteAsync(It.Is<UserGroup>(x => x.UserId == userId && x.GroupId == groupId)), Times.Never);
        }
        [Fact]
        public async Task DeleteGroup_UserGroupNotFound_ShouldNotDeleteGroupAndReturnFalse()
        {
            // Arrange
            string userId = "user1";
            int groupId = 1;
            var userGroups = new List<UserGroup>
            {
                new () { UserId = "user2", GroupId = 1 }
            };
            _userGroupRepositoryMock.Setup(x => x.GetAsync())
                                    .ReturnsAsync(userGroups);

            // Act
            var result = await _sut.DeleteAsync(userId, groupId);

            // Assert
            Assert.False(result);
            _userGroupRepositoryMock.Verify(x => x.DeleteAsync(It.Is<UserGroup>(x => x.UserId == userId && x.GroupId == groupId)), Times.Never);
        }
        [Fact]
        public async Task DeleteGroup_UserIsNotGroupCreator_ShouldNotDeleteGroupAndReturnFalse()
        {
            // Arrange
            string userId = "user1";
            int groupId = 1;
            string creatorId = "user2";
            var userGroups = new List<UserGroup>
            {
                new() { UserId = userId, GroupId = groupId, Group = new Group { CreatorId = creatorId} }
            };
            _userGroupRepositoryMock.Setup(x => x.GetAsync())
                                    .ReturnsAsync(userGroups);

            // Act
            var result = await _sut.DeleteAsync(userId, groupId);

            // Assert
            Assert.False(result);
            _userGroupRepositoryMock.Verify(x => x.DeleteAsync(It.Is<UserGroup>(x => x.UserId == userId && x.GroupId == groupId)), Times.Never);

        }
        [Fact]
        public async Task DeleteGroup_UserIsGroupCreator_ShouldDeleteGroupAndReturnTrue()
        {
            // Arrange
            string userId = "user1";
            int groupId = 1;
            string creatorId = "user1";

            var userGroups = new List<UserGroup>
            {
                new(){ UserId = userId, GroupId = groupId, Group = new Group { CreatorId = creatorId } }
            };
            _userGroupRepositoryMock.Setup(x => x.GetAsync())
                                    .ReturnsAsync(userGroups);

            _userGroupRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<UserGroup>()))
                                    .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.DeleteAsync(userId, groupId);

            // Assert
            Assert.True(result);
            _userGroupRepositoryMock.Verify(x => x.DeleteAsync(It.Is<UserGroup>(x => x.UserId == userId && x.GroupId == groupId)), Times.Once);
        }
        #endregion
    }
}