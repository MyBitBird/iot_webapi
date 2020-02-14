using FluentValidation;
using IOT.DTO;

namespace IOT
{
    public class AuthorizaValidator :  AbstractValidator<AuthorizeDTO>
    {
        public AuthorizaValidator()
        {
            RuleFor(x => x.Username).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
        }
    }
}