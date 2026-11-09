using Domain.ValueObject;
using Domain.ValueObject.Exceptions;
using Domain.ValueObject.Validators;
namespace MovieRatingsService.Domain.Tests.ValueObjects
{
    public class UsernameTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_NullOrWhiteSpace_ThrowsValueNullException(string? value)
        {
            Assert.Throws<ValueNullException>(() => new Username(value!));
        }

        [Fact]
        public void Constructor_OneCharacter_ThrowsValueTooShortException()
        {
            Assert.Throws<ValueTooShortException>(() => new Username("a"));
        }

        [Fact]
        public void Constructor_TooLong_ThrowsValueTooLongException()
        {
            var value = new string('a', UsernameValidator.MaxLength + 1);

            Assert.Throws<ValueTooLongException>(() => new Username(value));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(50)]
        public void Constructor_LengthWithinBounds_StoresValue(int length)
        {
            var value = new string('a', length);

            var username = new Username(value);

            Assert.Equal(value, username.Value);
        }
    }
}
