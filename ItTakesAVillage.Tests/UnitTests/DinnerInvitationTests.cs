using ItTakesAVillage.Contracts;
using ItTakesAVillage.Models;
using ItTakesAVillage.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItTakesAVillage.Tests.UnitTests
{
    public class DinnerInvitationTests
    {
        private readonly Mock<IRepository<DinnerInvitation>> _dinnerInvitationRepositoryMock;
        private readonly DinnerInvitationService _sut;
        public DinnerInvitationTests()
        {
            _dinnerInvitationRepositoryMock = new Mock<IRepository<DinnerInvitation>>();
            _sut = new DinnerInvitationService(_dinnerInvitationRepositoryMock.Object);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        public async Task Create_InvitationInFuture_ShouldReturnTrue(int days)
        {
            // Arrange
            var futureDate = DateTime.Now.AddDays(days);
            var dinnerInvitation = new DinnerInvitation { DateTime = futureDate };

            _dinnerInvitationRepositoryMock.Setup(x => x.AddAsync(It.IsAny<DinnerInvitation>()))
                                          .Returns(Task.CompletedTask);

            // Act
            var actual = await _sut.Create(dinnerInvitation);

            // Assert
            Assert.True(actual);
            _dinnerInvitationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<DinnerInvitation>()), Times.Once);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task Create_InvitationInPast_ShouldReturnFalse(int days)
        {
            // Arrange
            var pastDate = DateTime.Now.AddDays(days);
            var dinnerInvitation = new DinnerInvitation { DateTime = pastDate };

            // Act
            var actual = await _sut.Create(dinnerInvitation);

            // Assert
            Assert.False(actual);
            _dinnerInvitationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<DinnerInvitation>()), Times.Never);
        }
    }
}
