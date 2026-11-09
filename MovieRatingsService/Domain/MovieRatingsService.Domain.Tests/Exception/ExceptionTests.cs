using Domain.ValueObject.Exceptions;

namespace MovieRatingsService.Domain.Tests.Exception
{
    public class ExceptionTests
    {
        [Fact]
        public void EntityNotFoundException_MessageContainsEntityNameAndId()
        {
            var id = Guid.NewGuid();

            var ex = new EntityNotFoundException("User", id);

            Assert.Contains("User", ex.Message);
            Assert.Contains(id.ToString(), ex.Message);
        }

        [Fact]
        public void InvalidMovieDataException_KeepsMessage()
        {
            var ex = new InvalidMovieDataException("плохие данные");

            Assert.Equal("плохие данные", ex.Message);
        }

        [Fact]
        public void UnauthorizedOperationException_IsDomainOperationAndDomainException()
        {
            var ex = new UnauthorizedOperationException("нет прав");

            Assert.IsAssignableFrom<DomainOperationException>(ex);
            Assert.IsAssignableFrom<DomainException>(ex);
            Assert.Equal("нет прав", ex.Message);
        }

        [Fact]
        public void DomainArgumentNullException_WithParamNameOnly_BuildsDefaultMessage()
        {
            var ex = new DomainArgumentNullException("title");

            Assert.Equal("title", ex.ParamName);
            Assert.Contains("title", ex.Message);
        }

        [Fact]
        public void DomainArgumentOutOfRangeException_WithMessageOnly_KeepsParamName()
        {
            var ex = new DomainArgumentOutOfRangeException("age", "вне диапазона");

            Assert.Equal("age", ex.ParamName);
            Assert.Contains("вне диапазона", ex.Message);
        }

        [Fact]
        public void SmallScoreException_ContainsScoreAndMinimum()
        {
            var ex = new SmallScoreException(0, 1);

            Assert.Equal(0, ex.ActualValue);
            Assert.Contains("1", ex.Message);
        }

        [Fact]
        public void BigScoreException_ContainsScoreAndMaximum()
        {
            var ex = new BigScoreException(11, 10);

            Assert.Equal(11, ex.ActualValue);
            Assert.Contains("10", ex.Message);
        }

        [Fact]
        public void ValueTooLongException_ContainsParamAndLengths()
        {
            var ex = new ValueTooLongException("Title", 201, 200);

            Assert.Equal("Title", ex.ParamName);
            Assert.Equal(201, ex.ActualValue);
            Assert.Contains("200", ex.Message);
        }

        [Fact]
        public void ValueTooShortException_ContainsParamAndLengths()
        {
            var ex = new ValueTooShortException("Username", 1, 2);

            Assert.Equal("Username", ex.ParamName);
            Assert.Equal(1, ex.ActualValue);
        }

        [Fact]
        public void ValueTooBigException_ContainsParamAndLimit()
        {
            var ex = new ValueTooBigException("ReleaseYear", 3000, 2031);

            Assert.Equal("ReleaseYear", ex.ParamName);
            Assert.Contains("2031", ex.Message);
        }

        [Fact]
        public void ValueTooSmallException_ContainsParamAndLimit()
        {
            var ex = new ValueTooSmallException("ReleaseYear", 1800, 1888);

            Assert.Equal("ReleaseYear", ex.ParamName);
            Assert.Contains("1888", ex.Message);
        }

        [Fact]
        public void InvalidUrlException_ContainsInvalidValue()
        {
            var ex = new InvalidUrlException("PosterUrl", "not a url");

            Assert.Equal("PosterUrl", ex.ParamName);
            Assert.Contains("not a url", ex.Message);
        }

        [Fact]
        public void ValueNullException_MessageContainsParamName()
        {
            var ex = new ValueNullException("Title");

            Assert.Equal("Title", ex.ParamName);
            Assert.Contains("Title", ex.Message);
        }

        [Fact]
        public void ValidatorNullException_MessageContainsParamName()
        {
            var ex = new ValidatorNullException("validator");

            Assert.Equal("validator", ex.ParamName);
            Assert.Contains("validator", ex.Message);
        }
    }
}
