
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Customers.ValidationRules;
using System.Runtime.InteropServices;
using System;

namespace Business.Handlers.Customers.Commands
{


    public class UpdateCustomerCommand : Customer, IRequest<IResult>
    {


        public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, IResult>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IMediator _mediator;

            public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IMediator mediator)
            {
                _customerRepository = customerRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateCustomerValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
            {
                var isThereCustomerRecord = _customerRepository.Query().Any(u =>
                                                                                u.CustomerName == request.CustomerName &&
                                                                                u.Address == request.Address &&
                                                                                u.Email == request.Email &&
                                                                                !u.isDeleted);


                if (!isThereCustomerRecord)
                {

                    var isThereAnyCustomerRecord = await _customerRepository.GetAsync(u => u.Id == request.Id);
                    isThereAnyCustomerRecord.Status = true;
                    isThereAnyCustomerRecord.isDeleted = false;
                    isThereAnyCustomerRecord.LastUpdateDate = DateTime.Now;
                    isThereAnyCustomerRecord.CustomerName = request.CustomerName;
                    isThereAnyCustomerRecord.CustomerCode = request.CustomerCode;
                    isThereAnyCustomerRecord.Address = request.Address;
                    isThereAnyCustomerRecord.CustomerPhone = request.CustomerPhone;
                    isThereAnyCustomerRecord.Email = request.Email;


                    _customerRepository.Update(isThereAnyCustomerRecord);
                    await _customerRepository.SaveChangesAsync();
                    return new SuccessResult(Messages.Updated);
                }
                else
                {
                    return new ErrorResult(Messages.CustomerError);
                }

            }
        }
    }
}

