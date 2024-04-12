using ItTakesAVillage.Contracts;
using ItTakesAVillage.Models;
using ItTakesAVillage.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ItTakesAVillage.Tests.UnitTests
{
    public class NotificationTests
    {
        private readonly Mock<IGroupService> _groupServiceMock;
        private readonly Mock<IRepository<ItTakesAVillageUser>> _userRepositoryMock;
        private readonly Mock<IRepository<Notification>> _notificationRepositoryMock;
        private readonly NotificationService _sut;

        public NotificationTests()
        {
            _groupServiceMock = new Mock<IGroupService>();
            _userRepositoryMock = new Mock<IRepository<ItTakesAVillageUser>>();
            _notificationRepositoryMock = new Mock<IRepository<Notification>>();

            _sut = new NotificationService(_groupServiceMock.Object,
                                   _userRepositoryMock.Object,
                                   _notificationRepositoryMock.Object);
        }
        [Fact]
        public async Task CountAsync_NotificationsExist_ShouldReturnCorrectCount()
        {
            // Arrange
            var userId = "testUserId";
            var unreadNotifications = new List<Notification>
            {
                new Notification { UserId = userId, IsRead = false },
                new Notification { UserId = userId, IsRead = false },
            };

            _notificationRepositoryMock.Setup(x => x.GetByFilterAsync(It.IsAny<Expression<Func<Notification, bool>>>()))
                                      .ReturnsAsync(unreadNotifications);

            // Act
            var actual = await _sut.CountAsync(userId);

            // Assert
            Assert.Equal(unreadNotifications.Count, actual);
            _notificationRepositoryMock.Verify(x => x.GetByFilterAsync(It.IsAny<Expression<Func<Notification, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task CountAsync_NoNotifications_ShouldReturnZero()
        {
            // Arrange
            var userId = "testUserId";
            var emptyList = new List<Notification>();

            _notificationRepositoryMock.Setup(x => x.GetByFilterAsync(It.IsAny<Expression<Func<Notification, bool>>>()))
                                      .ReturnsAsync(emptyList);

            // Act
            var actual = await _sut.CountAsync(userId);

            // Assert
            Assert.Equal(0, actual);
            _notificationRepositoryMock.Verify(x => x.GetByFilterAsync(It.IsAny<Expression<Func<Notification, bool>>>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(DinnerInvitationTestDataExistingCreator))]
        [MemberData(nameof(PlayDateTestDataExistingCreator))]
        public async Task CreateAsync_CreatorExists_ShouldAddNotificationWithCreatorName<TEventObject>(TEventObject eventObject, string userId, ItTakesAVillageUser creator)
        where TEventObject : BaseEvent
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.GetAsync(eventObject.CreatorId))
                               .ReturnsAsync(creator);

            _notificationRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Notification>()))
                                        .Returns(Task.CompletedTask);

            // Act
            await _sut.CreateAsync(eventObject, userId, x => x.CreatorId);

            // Assert
            _notificationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Notification>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(DinnerInvitationTestDataNonExistingCreator))]
        [MemberData(nameof(PlayDateTestDataNonExistingCreator))]
        public async Task CreateAsync_CreatorDoesNotExist_ShouldAddNotificationWithUnknownCreator<TEventObject>(TEventObject eventObject, string userId)
        where TEventObject : BaseEvent
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.GetAsync(eventObject.CreatorId))
                               .ReturnsAsync(null as ItTakesAVillageUser);

            _notificationRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Notification>()))
                                        .Returns(Task.CompletedTask);

            // Act
            await _sut.CreateAsync(eventObject, userId, x => x.CreatorId);

            // Assert
            _notificationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Notification>()), Times.Once);
        }

        [Fact]
        public async Task UpdateIsReadAsync_NotificationExists_ShouldUpdateAndCallUpdateAsync()
        {
            // Arrange
            var notificationId = 1;
            var existingNotification = new Notification { Id = notificationId, IsRead = false };

            _notificationRepositoryMock.Setup(x => x.GetAsync(notificationId))
                                      .ReturnsAsync(existingNotification);

            // Act
            await _sut.UpdateIsReadAsync(notificationId);

            // Assert
            Assert.True(existingNotification.IsRead);
            _notificationRepositoryMock.Verify(x => x.UpdateAsync(existingNotification), Times.Once);
        }

        [Fact]
        public async Task UpdateIsReadAsync_NotificationDoesNotExist_ShouldNotCallUpdateAsync()
        {
            // Arrange
            var nonExistentNotificationId = 999;

            _notificationRepositoryMock.Setup(x => x.GetAsync(nonExistentNotificationId))
                                      .ReturnsAsync(null as Notification);
            // Act
            await _sut.UpdateIsReadAsync(nonExistentNotificationId);

            // Assert
            _notificationRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Notification>()), Times.Never);
        }
        public static IEnumerable<object[]> DinnerInvitationTestDataExistingCreator()
        {
            yield return new object[] { new DinnerInvitation { CreatorId = "creatorId" }, "testUserId", new ItTakesAVillageUser { Id = "creatorId", FirstName = "John", LastName = "Doe" } };
        }
        public static IEnumerable<object[]> PlayDateTestDataExistingCreator()
        {
            yield return new object[] { new PlayDate { CreatorId = "creatorId" }, "testUserId", new ItTakesAVillageUser { Id = "creatorId", FirstName = "Jane", LastName = "Doe" } };
        }
        public static IEnumerable<object[]> DinnerInvitationTestDataNonExistingCreator()
        {
            yield return new object[] { new DinnerInvitation { CreatorId = "testUserId" }, "testUserId" };
        }
        public static IEnumerable<object[]> PlayDateTestDataNonExistingCreator()
        {
            yield return new object[] { new PlayDate { CreatorId = "testUserId" }, "testUserId" };
        }
    }
}
