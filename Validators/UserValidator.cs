using FluentValidation;
using IOT.DTO;

namespace IOT.Validators
{
    public class UserValidator : AbstractValidator<UserDTO>
    {

        public UserValidator()
        {
            RuleFor(x=>x.Username).NotEmpty().NotNull();
            RuleFor(x=>x.Password).NotEmpty().NotNull().Length(5,20);
            RuleFor(x=>x.Name).NotEmpty().NotNull();
            RuleFor(x=>x.Family).NotEmpty().NotNull();
        }
    }

}