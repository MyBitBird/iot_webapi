using FluentValidation;
using IOT.DTO;

namespace IOT.Validators
{
    public class ServiceValidator : AbstractValidator<ServiceDTO>
    {
        public ServiceValidator()
        {
            RuleFor(x=>x.title).NotEmpty().NotNull();
        }
    }

}