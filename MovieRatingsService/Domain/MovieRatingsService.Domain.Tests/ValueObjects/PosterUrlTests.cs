using Domain.ValueObject.Exceptions;
using Domain.ValueObject.Validators;
using Domain.ValueObject;

namespace MovieRatingsService.Domain.Tests.ValueObjects
{
    public class PosterUrlTests
    {
        private const string Prefix = "http://a.com/";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_NullOrWhiteSpace_ThrowsValueNullException(string? value)
        {
            Assert.Throws<ValueNullException>(() => new PosterUrl(value!));
        }

        [Theory]
        [InlineData("not a url")]
        [InlineData("just-text")]
        public void Constructor_NotAbsoluteUrl_ThrowsInvalidUrlException(string value)
        {
            Assert.Throws<InvalidUrlException>(() => new PosterUrl(value));
        }

        [Fact]
        public void Constructor_TooLong_ThrowsValueTooLongException()
        {
            var value = Prefix + new string('a', PosterUrlValidator.MaxLength + 1 - Prefix.Length);

            Assert.Throws<ValueTooLongException>(() => new PosterUrl(value));
        }

        [Fact]
        public void Constructor_MaxLength_StoresValue()
        {
            var value = Prefix + new string('a', PosterUrlValidator.MaxLength - Prefix.Length);

            Assert.Equal(value, new PosterUrl(value).Value);
        }

        [Theory]
        [InlineData("https://example.com/poster.jpg")]
        [InlineData("http://localhost/img.png")]
        public void Constructor_ValidAbsoluteUrl_StoresValue(string value)
        {
            Assert.Equal(value, new PosterUrl(value).Value);
        }
    }
}
