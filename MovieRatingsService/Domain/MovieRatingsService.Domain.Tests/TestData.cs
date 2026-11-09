using Domain.ValueObject;
using MovieRatingsService.Domain.Domain.Entities;

namespace MovieRatingsService.Domain.Tests
{
    internal static class TestData
    {
        public static User NewUser(string name = "alice")
            => new(new Username(name));

        public static Admin NewAdmin(string name = "root", string permissions = "manage_movies,moderate_reviews")
            => new(new Username(name), new Permissions(permissions));

        public static Movie NewMovie(Admin? admin = null, string title = "Inception")
            => (admin ?? NewAdmin()).CreateMovie(new Title(title));
    }
}
