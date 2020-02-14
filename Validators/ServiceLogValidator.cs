using FluentValidation;
using IOT.DTO;

namespace IOT.Validators
{
    public class ServiceLogValidator : AbstractValidator<ServiceLogDTO>
    {
        public ServiceLogValidator()
        {
            RuleFor(x=>x.ServiceId).NotEmpty().NotNull();
            RuleFor(x=>x.LogDate).NotEmpty().NotNull();
            RuleFor(x=>x.ServiceData).NotEmpty().NotNull();
            RuleForEach(x=>x.ServiceData).SetValidator(new ServiceDataValidator());
        }
    }
}