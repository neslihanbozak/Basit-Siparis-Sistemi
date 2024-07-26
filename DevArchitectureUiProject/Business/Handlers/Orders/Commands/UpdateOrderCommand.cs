
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.Orders.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.Orders.Commands
{


    public class UpdateOrderCommand : Order, IRequest<IResult>
    {

        public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, IResult>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IMediator _mediator;

            public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
            {
                _orderRepository = orderRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateOrderValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            {
                var isThereOrderRecord = await _orderRepository.GetAsync(u => u.Id == request.Id);


                //isThereOrderRecord.LastUpdateUserId = request.LastUpdateUserId;
                //isThereOrderRecord.LastUpdateDate = DateTime.Now;
                //isThereOrderRecord.Status = request.Status;
                //isThereOrderRecord.isDeleted = request.isDeleted;
                isThereOrderRecord.CustomerId = request.CustomerId;
                isThereOrderRecord.StorageId = request.StorageId;
                isThereOrderRecord.Quantity = request.Quantity;


                _orderRepository.Update(isThereOrderRecord);
                await _orderRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

