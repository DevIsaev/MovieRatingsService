using Domain.ValueObject.Exceptions;
using Domain.ValueObject.Validators;
using Domain.ValueObject;

namespace MovieRatingsService.Domain.Tests.ValueObjects
{
    public class DescriptionTests
    {
        [Fact]
        public void Constructor_Null_IsAllowed()
        {
            var description = new Description(null);

            Assert.Null(description.Value);
        }

        [Fact]
        public void Constructor_EmptyString_IsAllowed()
        {
            Assert.Equal("", new Description("").Value);
        }

        [Fact]
        public void Constructor_TooLong_ThrowsValueTooLongException()
        {
            var value = new string('a', DescriptionValidator.MaxLength + 1);

            Assert.Throws<ValueTooLongException>(() => new Description(value));
        }

        [Fact]
        public void Constructor_MaxLength_StoresValue()
        {
            var value = new string('a', DescriptionValidator.MaxLength);

            Assert.Equal(value, new Description(value).Value);
        }
    }
}
