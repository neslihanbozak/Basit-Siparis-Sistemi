using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Orders.ValidationRules;
using System;
using Business.Handlers.Storages.Commands;

namespace Business.Handlers.Orders.Commands
{
    /// <summary>
    /// 
    /// </summary>

    public class CreateOrderCommand : Order, MediatR.IRequest<IResult>
    {

        public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, IResult>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IMediator _mediator;
            public CreateOrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
            {
                _orderRepository = orderRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateOrderValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                //var isThereOrderRecord = _orderRepository.Query().Any(u => u.CreatedUserId == request.CreatedUserId);

                //if (isThereOrderRecord == true)
                //    return new ErrorResult(Messages.NameAlreadyExist);

                var addedOrder = new Order
                {
                    //CreatedUserId = request.CreatedUserId,
                    //CreatedDate = DateTime.Now,
                    //Status = request.Status,
                    CustomerId = request.CustomerId,
                    StorageId = request.StorageId,
                    Quantity = request.Quantity,

                };

                _orderRepository.Add(addedOrder);
                await _orderRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}
