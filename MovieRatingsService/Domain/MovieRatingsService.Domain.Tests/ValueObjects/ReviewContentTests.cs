using Domain.ValueObject.Exceptions;
using Domain.ValueObject.Validators;
using Domain.ValueObject;

namespace MovieRatingsService.Domain.Tests.ValueObjects
{
    public class ReviewContentTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_NullOrWhiteSpace_ThrowsValueNullException(string? value)
        {
            Assert.Throws<ValueNullException>(() => new ReviewContent(value!));
        }

        [Fact]
        public void Constructor_TooLong_ThrowsValueTooLongException()
        {
            var value = new string('a', ReviewContentValidator.MaxLength + 1);

            Assert.Throws<ValueTooLongException>(() => new ReviewContent(value));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2000)]
        public void Constructor_LengthWithinBounds_StoresValue(int length)
        {
            var value = new string('a', length);

            Assert.Equal(value, new ReviewContent(value).Value);
        }
    }
}
