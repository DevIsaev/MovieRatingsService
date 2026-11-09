using Domain.ValueObject.Exceptions;
using Domain.ValueObject;

namespace MovieRatingsService.Domain.Tests.ValueObjects
{
    public class PermissionsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_NullOrWhiteSpace_ThrowsValueNullException(string? value)
        {
            Assert.Throws<ValueNullException>(() => new Permissions(value!));
        }

        [Theory]
        [InlineData("manage_movies")]
        [InlineData("manage_movies,moderate_reviews")]
        public void Constructor_NotEmpty_StoresValue(string value)
        {
            Assert.Equal(value, new Permissions(value).Value);
        }
    }
}
