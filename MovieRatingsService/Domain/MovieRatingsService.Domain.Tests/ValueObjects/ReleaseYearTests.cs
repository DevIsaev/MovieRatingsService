using Domain.ValueObject.Exceptions;
using Domain.ValueObject.Validators;
using Domain.ValueObject;

namespace MovieRatingsService.Domain.Tests.ValueObjects
{
    public class ReleaseYearTests
    {
        [Theory]
        [InlineData(1887)]
        [InlineData(0)]
        [InlineData(-5)]
        public void Constructor_BeforeMinYear_ThrowsValueTooSmallException(int value)
        {
            Assert.Throws<ValueTooSmallException>(() => new ReleaseYear(value));
        }

        [Fact]
        public void Constructor_MinYear_StoresValue()
        {
            Assert.Equal(ReleaseYearValidator.MinYear, new ReleaseYear(ReleaseYearValidator.MinYear).Value);
        }

        [Fact]
        public void Constructor_CurrentYearPlusFive_StoresValue()
        {
            var year = DateTime.UtcNow.Year + 5;

            Assert.Equal(year, new ReleaseYear(year).Value);
        }

        [Fact]
        public void Constructor_CurrentYearPlusSix_ThrowsValueTooBigException()
        {
            var year = DateTime.UtcNow.Year + 6;

            Assert.Throws<ValueTooBigException>(() => new ReleaseYear(year));
        }
    }
}
