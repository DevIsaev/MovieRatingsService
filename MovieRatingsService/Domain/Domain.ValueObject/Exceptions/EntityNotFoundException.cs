namespace Domain.ValueObject.Exceptions
{
    public class EntityNotFoundException(string entityName, object id)
           : DomainException($"{entityName} с идентификатором {id} не найден.");
}
