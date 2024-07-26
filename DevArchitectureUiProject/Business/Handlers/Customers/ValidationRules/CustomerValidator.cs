
using Business.Handlers.Customers.Commands;
using FluentValidation;

namespace Business.Handlers.Customers.ValidationRules
{

    public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.CustomerName).NotEmpty();    
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.CustomerPhone).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();

        }
    }
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(x => x.CustomerName).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.CustomerPhone).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();

        }
    }
}