using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{

    // Права администратора (строка с правами, разделёнными запятыми)

    public class Permissions(string value) : ValueObject<string>(new PermissionsValidator(), value);
}
