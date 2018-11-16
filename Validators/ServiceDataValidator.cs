using System;
using IOT.DTO;
using FluentValidation;

namespace IOT.Validators
{
    public class ServiceDataValidator : AbstractValidator<DeviceDataDTO>
    {
        public ServiceDataValidator()
        {
            RuleFor(x=>x.Code).NotNull().NotEmpty();
            
            RuleFor(x => x.Data).NotNull().NotEmpty();

            RuleFor(x => x.ServiceProperty).Null();

        }
    }
}