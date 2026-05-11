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
}
