namespace ItTakesAVillage.Tests.UnitTests
{
    public class ToolLoanTests
    {
        private readonly Mock<IRepository<ToolLoan>> _toolLoanRepositoryMock;
        private readonly ToolLoanService _sut;
        public ToolLoanTests()
        {
            _toolLoanRepositoryMock = new Mock<IRepository<ToolLoan>>();
            _sut = new ToolLoanService(_toolLoanRepositoryMock.Object);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(-100, 0)]
        [InlineData(0, -1)]
        [InlineData(0, -100)]
        [InlineData(50, 0)]
        [InlineData(-1, -1)]
        [InlineData(-100, -100)]
        public async Task Create_ToolLoanInPast_ShouldReturnFalse(int daysFrom, int daysTo)
        {
            //Arrange
            var pastFromDate= DateOnly.FromDateTime(DateTime.Today).AddDays(daysFrom);
            var pastToDate= DateOnly.FromDateTime(DateTime.Today).AddDays(daysTo);
            var loan = new ToolLoan { FromDate = pastFromDate , ToDate = pastToDate, ToolPool = new ToolPool()};
            
            //Act
            var actual = await _sut.Create(loan);

            //Assert
            Assert.False(actual);
            _toolLoanRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ToolLoan>()), Times.Never);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(100, 100)]
        public async Task Create_ToolLoanInFuture_ShouldReturnTrue(int daysFrom, int daysTo)
        {
            //Arrange
            var pastFromDate= DateOnly.FromDateTime(DateTime.Today).AddDays(daysFrom);
            var pastToDate= DateOnly.FromDateTime(DateTime.Today).AddDays(daysTo);
            var loan = new ToolLoan { FromDate = pastFromDate , ToDate = pastToDate, ToolPool = new ToolPool()};
            
            //Act
            var actual = await _sut.Create(loan);

            //Assert
            Assert.True(actual);
            _toolLoanRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ToolLoan>()), Times.Once);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(-100, 0)]
        [InlineData(0, -1)]
        [InlineData(0, -100)]
        [InlineData(50, 0)]
        [InlineData(-1, -1)]
        [InlineData(-100, -100)]
        public async Task Update_ToolLoanInPast_ShouldReturnFalse(int daysFrom, int daysTo)
        {
            //Arrange
            var pastFromDate = DateOnly.FromDateTime(DateTime.Today).AddDays(daysFrom);
            var pastToDate = DateOnly.FromDateTime(DateTime.Today).AddDays(daysTo);
            var loan = new ToolLoan { FromDate = pastFromDate, ToDate = pastToDate, ToolPool = new ToolPool()};

            //Act
            var actual = await _sut.Update(loan);

            //Assert
            Assert.False(actual);
            _toolLoanRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ToolLoan>()), Times.Never);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(100, 100)]
        public async Task Update_ToolLoanInFuture_ShouldReturnTrue(int daysFrom, int daysTo)
        {
            //Arrange
            var pastFromDate = DateOnly.FromDateTime(DateTime.Today).AddDays(daysFrom);
            var pastToDate = DateOnly.FromDateTime(DateTime.Today).AddDays(daysTo);
            var loan = new ToolLoan { FromDate = pastFromDate, ToDate = pastToDate, ToolPool = new ToolPool() };

            //Act
            var actual = await _sut.Create(loan);

            //Assert
            Assert.True(actual);
            _toolLoanRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ToolLoan>()), Times.Once);
        }
    }
}