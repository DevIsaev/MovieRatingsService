using Domain.ValueObject.Exceptions;
using Domain.ValueObject;
using MovieRatingsService.Domain.Domain.Entities;

namespace MovieRatingsService.Domain.Tests.Entities
{
    public class UserTests
    {
        [Fact]
        public void Constructor_ValidUsername_SetsUsernameAndGeneratesId()
        {
            var username = new Username("alice");

            var user = new User(username);

            Assert.Same(username, user.Username);
            Assert.NotEqual(Guid.Empty, user.Id);
        }

        [Fact]
        public void Constructor_TwoUsers_HaveDifferentIds()
        {
            Assert.NotEqual(TestData.NewUser().Id, TestData.NewUser().Id);
        }

        [Fact]
        public void Constructor_NullUsername_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => new User(null!));
        }

        [Fact]
        public void UpdateUsername_NewName_ChangesUsernameAndReturnsTrue()
        {
            var user = TestData.NewUser("alice");

            var changed = user.UpdateUsername(new Username("bob"));

            Assert.True(changed);
            Assert.Equal("bob", user.Username.Value);
        }

        [Fact]
        public void UpdateUsername_SameName_ReturnsFalse()
        {
            var user = TestData.NewUser("alice");

            var changed = user.UpdateUsername(new Username("alice"));

            Assert.False(changed);
        }

        [Fact]
        public void UpdateUsername_Null_ThrowsDomainArgumentNullException()
        {
            var user = TestData.NewUser();

            Assert.Throws<DomainArgumentNullException>(() => user.UpdateUsername(null!));
        }

        [Fact]
        public void ToString_ContainsUsernameAndId()
        {
            var user = TestData.NewUser("alice");

            Assert.Equal($"alice (ID: {user.Id})", user.ToString());
        }
    }
}
