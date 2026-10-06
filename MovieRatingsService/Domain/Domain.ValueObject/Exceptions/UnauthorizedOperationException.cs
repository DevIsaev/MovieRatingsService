namespace Domain.ValueObject.Exceptions
{
    // Операция запрещена из за недостатка прав
    public class UnauthorizedOperationException(string message) : DomainOperationException(message);
}
