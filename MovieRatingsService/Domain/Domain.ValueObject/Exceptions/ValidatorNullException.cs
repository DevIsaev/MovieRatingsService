namespace Domain.ValueObject.Exceptions
{
    // validator не передан в ValueObject.
    public class ValidatorNullException : DomainArgumentNullException
    {
        public ValidatorNullException(string paramName)
            : base(paramName, $"Валидатор для '{paramName}' не может быть null.") { }
    }
}
