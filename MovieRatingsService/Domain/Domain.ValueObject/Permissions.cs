using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{

    // Права администратора (строка с правами, разделёнными запятыми).

    public class Permissions : ValueObject<string>
    {
        private static readonly IValidator<string> _defaultValidator = new PermissionsValidator();

        public Permissions(string value) : this(_defaultValidator, value) { }
        public Permissions(IValidator<string> validator, string value) : base(validator, value) { }
    }
}
