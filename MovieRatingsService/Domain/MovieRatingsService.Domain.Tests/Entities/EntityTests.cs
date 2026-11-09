using MovieRatingsService.Domain.Base;
using MovieRatingsService.Domain.Domain.Entities;

namespace MovieRatingsService.Domain.Tests.Entities
{
    // protected-конструкторы без параметров нужны EF Core
    public class EntityTests
    {
        [Theory]
        [InlineData(typeof(User))]
        [InlineData(typeof(Admin))]
        [InlineData(typeof(Movie))]
        [InlineData(typeof(Rating))]
        [InlineData(typeof(Review))]
        public void ProtectedParameterlessConstructor_CreatesEntityWithEmptyId(Type entityType)
        {
            var entity = (Entity<Guid>)Activator.CreateInstance(entityType, nonPublic: true)!;

            Assert.Equal(Guid.Empty, entity.Id);
        }
    }
}
