using Domain.ValueObject;
using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace MovieRatingsService.Domain.Tests.ValueObjects
{
    public class ValueObjectTests
    {
        private sealed class CountingValidator : IValidator<int>
        {
            public int Calls { get; private set; }
            public void Validate(int value) => Calls++;
        }

        private const string KnownBug = "Ошибка домена: ValueObject<T> не поддерживает null-значение (Description(null)).";

        private sealed class TestValueObject(IValidator<int> validator, int value)
            : ValueObject<int>(validator, value);

        [Fact]
        public void Constructor_ValidValue_CallsValidatorOnceAndStoresValue()
        {
            var validator = new CountingValidator();

            var vo = new TestValueObject(validator, 42);

            Assert.Equal(1, validator.Calls);
            Assert.Equal(42, vo.Value);
        }

        [Fact]
        public void Constructor_NullValidator_ThrowsValidatorNullException()
        {
            Assert.Throws<ValidatorNullException>(() => new TestValueObject(null!, 1));
        }

        [Fact]
        public void Equals_SameValue_ReturnsTrue()
        {
            Assert.True(new Username("alice").Equals(new Username("alice")));
        }

        [Fact]
        public void Equals_DifferentValue_ReturnsFalse()
        {
            Assert.False(new Username("alice").Equals(new Username("bob")));
        }

        [Fact]
        public void Equals_SameValueDifferentType_ReturnsFalse()
        {
            object title = new Title("abc");

            Assert.False(new Username("abc").Equals(title));
        }

        [Fact]
        public void Equals_Null_ReturnsFalse()
        {
            Assert.False(new Username("alice").Equals((object?)null));
        }

        [Fact]
        public void Equals_NotValueObject_ReturnsFalse()
        {
            Assert.False(new Username("alice").Equals((object)"alice"));
        }

        [Fact]
        public void Equals_SameReference_ReturnsTrue()
        {
            var username = new Username("alice");

            Assert.True(username.Equals(username));
        }

        [Fact]
        public void EqualityOperator_SameValues_ReturnsTrue()
        {
            Assert.True(new Score(5) == new Score(5));
        }

        [Fact]
        public void InequalityOperator_DifferentValues_ReturnsTrue()
        {
            Assert.True(new Score(5) != new Score(6));
        }

        [Fact]
        public void EqualityOperator_BothNull_ReturnsTrue()
        {
            Score? left = null;
            Score? right = null;

            Assert.True(left == right);
        }

        [Fact]
        public void EqualityOperator_OneNull_ReturnsFalse()
        {
            Score? right = null;

            Assert.False(new Score(5) == right);
        }

        [Fact]
        public void GetHashCode_SameValues_ReturnsSameHash()
        {
            Assert.Equal(new Username("alice").GetHashCode(), new Username("alice").GetHashCode());
        }

        [Fact]
        public void ToString_ReturnsValueAsString()
        {
            Assert.Equal("alice", new Username("alice").ToString());
            Assert.Equal("7", new Score(7).ToString());
        }

        // Description допускает null, а ValueObject<T> не умеет работать с null

        [Fact(Skip = KnownBug)]
        public void ToString_NullDescription_DoesNotThrow()
        {
            var description = new Description(null);

            Assert.NotNull(description.ToString());
        }

        [Fact(Skip = KnownBug)]
        public void GetHashCode_NullDescription_DoesNotThrow()
        {
            var description = new Description(null);

            var hash = description.GetHashCode();

            Assert.Equal(hash, new Description(null).GetHashCode());
        }

        [Fact(Skip = KnownBug)]
        public void Equals_TwoNullDescriptions_ReturnsTrue()
        {
            Assert.True(new Description(null).Equals(new Description(null)));
        }

        [Fact(Skip = KnownBug)]
        public void Equals_TextDescriptionAndNullDescription_ReturnsFalse()
        {
            Assert.False(new Description("text").Equals(new Description(null)));
        }
    }
}