namespace Domain.ValueObject.Exceptions
{
    // Исключение для случев, когда аргумент равен null
    public class DomainArgumentNullException : ArgumentNullException
    {
        public DomainArgumentNullException(string paramName)
            : base(paramName, BuildMessage(paramName)) { }

        public DomainArgumentNullException(string paramName, string message)
            : base(paramName, message) { }

        private static string BuildMessage(string paramName)
            => $"Параметр '{paramName}' не может быть null или пустым.";
    }


    // validator не передан в ValueObject.
    public class ValidatorNullException : DomainArgumentNullException
    {
        public ValidatorNullException(string paramName)
            : base(paramName, $"Валидатор для '{paramName}' не может быть null.") { }
    }


    // ValueObject null или пустое.
    public class ValueNullException : DomainArgumentNullException
    {
        public ValueNullException(string paramName)
            : base(paramName, $"Значение '{paramName}' не может быть null или состоять только из пробелов.") { }
    }
}
