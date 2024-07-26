
using Business.Handlers.Storages.Commands;
using FluentValidation;

namespace Business.Handlers.Storages.ValidationRules
{

    public class CreateStorageValidator : AbstractValidator<CreateStorageCommand>
    {
        public CreateStorageValidator()
        {

        }
    }
    public class UpdateStorageValidator : AbstractValidator<UpdateStorageCommand>
    {
        public UpdateStorageValidator()
        {

        }
    }
}