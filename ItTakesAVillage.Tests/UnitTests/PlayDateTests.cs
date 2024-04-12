using ItTakesAVillage.Contracts;
using ItTakesAVillage.Models;
using ItTakesAVillage.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItTakesAVillage.Tests.UnitTests
{
    public class PlayDateTests
    {
        private readonly Mock<IRepository<PlayDate>> _playDateRepositoryMock;
        private readonly PlayDateService _sut;

        public PlayDateTests()
        {
            _playDateRepositoryMock = new Mock<IRepository<PlayDate>>();
            _sut = new PlayDateService(_playDateRepositoryMock.Object);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        public async Task Create_InvitationInFuture_ShouldReturnTrue(int days)
        {
            // Arrange
            var futureDate = DateTime.Now.AddDays(days);
            var playDate = new PlayDate { DateTime = futureDate };

            _playDateRepositoryMock.Setup(x => x.AddAsync(It.IsAny<PlayDate>()))
                                          .Returns(Task.CompletedTask);

            // Act
            var actual = await _sut.Create(playDate);

            // Assert
            Assert.True(actual);
            _playDateRepositoryMock.Verify(x => x.AddAsync(It.IsAny<PlayDate>()), Times.Once);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task Create_InvitationInPast_ShouldReturnFalse(int days)
        {
            // Arrange
            var pastDate = DateTime.Now.AddDays(days);
            var playDate = new PlayDate { DateTime = pastDate };

            // Act
            var actual = await _sut.Create(playDate);

            // Assert
            Assert.False(actual);
            _playDateRepositoryMock.Verify(x => x.AddAsync(It.IsAny<PlayDate>()), Times.Never);
        }

    }
}
