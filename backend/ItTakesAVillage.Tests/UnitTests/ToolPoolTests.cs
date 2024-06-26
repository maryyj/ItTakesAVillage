﻿namespace ItTakesAVillage.Tests.UnitTests
{
    public class ToolPoolTests
    {
        private readonly Mock<IRepository<ToolPool>> _toolPoolRepositoryMock;
        private readonly Mock<IEventService<ToolLoan>> _toolLoanServiceMock;
        private readonly Mock<IRepository<UserGroup>> _userGroupRepositoryMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly ToolPoolService _sut;
        public ToolPoolTests()
        {
            _toolPoolRepositoryMock = new Mock<IRepository<ToolPool>>();
            _userGroupRepositoryMock = new Mock<IRepository<UserGroup>>();
            _toolLoanServiceMock = new Mock<IEventService<ToolLoan>>();
            _notificationServiceMock = new Mock<INotificationService>();


        _sut = new ToolPoolService(_toolPoolRepositoryMock.Object,
                _userGroupRepositoryMock.Object,
                _notificationServiceMock.Object,
                _toolLoanServiceMock.Object);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task Create_ToolShareInPast_ShouldReturnFalse(int days)
        {
            //Arrange
            var pastDate = DateTime.Now.AddDays(days);
            var tool = new ToolPool { DateTime = pastDate };

            //Act
            var actual = await _sut.CreateAsync(tool);
            //Assert
            Assert.False(actual);
            _toolPoolRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ToolPool>()), Times.Never);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        public async Task Create_ToolShareInFuture_ShouldReturnTrue(int days)
        {
            // Arrange
            var futureDate = DateTime.Now.AddDays(days);
            var tool = new ToolPool { DateTime = futureDate };

            _toolPoolRepositoryMock.Setup(x => x.AddAsync(It.IsAny<ToolPool>()))
                                          .Returns(Task.CompletedTask);
            // Act
            var actual = await _sut.CreateAsync(tool);
            // Assert
            Assert.True(actual);
            _toolPoolRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ToolPool>()), Times.Once);
        }
        [Fact]
        public async Task Delete_ToolIsBorrowed_ShouldReturnFalse()
        {
            int toolId = 123;
            var borrowedTool = new ToolPool { Id = toolId, IsBorrowed = true };
            _toolPoolRepositoryMock.Setup(x => x.GetAsync(toolId)).ReturnsAsync(borrowedTool);

            // Act
            var result = await _sut.DeleteAsync(toolId);

            // Assert
            Assert.False(result);
            _toolPoolRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<ToolPool>()), Times.Never);
        }
        [Fact]
        public async Task Delete_ToolIsNotBorrowed_ShouldReturnTrue()
        {
            int toolId = 123;
            var borrowedTool = new ToolPool { Id = toolId, IsBorrowed = false };
            _toolPoolRepositoryMock.Setup(x => x.GetAsync(toolId)).ReturnsAsync(borrowedTool);

            // Act
            var result = await _sut.DeleteAsync(toolId);

            // Assert
            Assert.True(result);
            _toolPoolRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<ToolPool>()), Times.Once);
        }
        [Fact]
        public void ValidateReturnDate_LoanExpired_SetsIsBorrowedToFalseAndIsReturnedToTrue()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);

            var expiredLoan = new ToolLoan
            {
                ToDate = today.AddDays(-1),
                IsReturned = false,
                ToolPool = new ToolPool { IsBorrowed = true }
            };

            var loans = new List<ToolLoan> { expiredLoan };

            // Act
            _sut.ValidateReturnDate(loans);

            // Assert
            Assert.False(expiredLoan.ToolPool.IsBorrowed);
            Assert.True(expiredLoan.IsReturned);
        }
    }
}
