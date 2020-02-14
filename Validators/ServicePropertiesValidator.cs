using FluentValidation;
using IOT.DTO;

namespace IOT.Validators
{
    public class ServicePropertiesValidator : AbstractValidator<ServicePropertiesDTO>
    {
        public ServicePropertiesValidator()
        {
            RuleFor(x=>x.Title).NotNull();
            RuleFor(x=>x.Code).NotNull().NotEmpty();
        }
    }
}