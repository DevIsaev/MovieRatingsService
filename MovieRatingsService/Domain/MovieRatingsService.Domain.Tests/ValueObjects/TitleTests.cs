using Domain.ValueObject;
using Domain.ValueObject.Exceptions;
using Domain.ValueObject.Validators;
namespace MovieRatingsService.Domain.Tests.ValueObjects
{
    public class TitleTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_NullOrWhiteSpace_ThrowsValueNullException(string? value)
        {
            Assert.Throws<ValueNullException>(() => new Title(value!));
        }

        [Fact]
        public void Constructor_TooLong_ThrowsValueTooLongException()
        {
            var value = new string('a', TitleValidator.MaxLength + 1);

            Assert.Throws<ValueTooLongException>(() => new Title(value));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(200)]
        public void Constructor_LengthWithinBounds_StoresValue(int length)
        {
            var value = new string('a', length);

            Assert.Equal(value, new Title(value).Value);
        }
    }
}
