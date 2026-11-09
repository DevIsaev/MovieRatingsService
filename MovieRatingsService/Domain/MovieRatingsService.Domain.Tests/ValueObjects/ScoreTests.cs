using Domain.ValueObject.Exceptions;
using Domain.ValueObject;

namespace MovieRatingsService.Domain.Tests.ValueObjects
{
    public class ScoreTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Constructor_BelowMinimum_ThrowsSmallScoreException(int value)
        {
            Assert.Throws<SmallScoreException>(() => new Score(value));
        }

        [Theory]
        [InlineData(11)]
        [InlineData(100)]
        public void Constructor_AboveMaximum_ThrowsBigScoreException(int value)
        {
            Assert.Throws<BigScoreException>(() => new Score(value));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void Constructor_WithinBounds_StoresValue(int value)
        {
            Assert.Equal(value, new Score(value).Value);
        }
    }
}
