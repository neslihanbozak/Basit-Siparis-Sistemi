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
using Business.Handlers.Products.ValidationRules;
using System;

namespace Business.Handlers.Products.Commands
{


    public class UpdateProductCommand : Product, IRequest<IResult>
    {

        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, IResult>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMediator _mediator;

            public UpdateProductCommandHandler(IProductRepository productRepository, IMediator mediator)
            {
                _productRepository = productRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateProductValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                var isThereProductRecord = _productRepository.Query().Any(u =>
                                                                                u.ProductName == request.ProductName &&
                                                                                u.Color == request.Color &&
                                                                                u.Size == request.Size &&
                                                                                !u.isDeleted
                                                                          );

                if (isThereProductRecord)
                {
                    return new ErrorResult(Messages.MainError);
                }
                else
                {
                    var isThereAnyProductRecord = await _productRepository.GetAsync(product => product.Id == request.Id);
                    isThereAnyProductRecord.LastUpdateDate = DateTime.Now;
                    isThereAnyProductRecord.LastUpdateUserId = request.LastUpdateUserId;
                    isThereAnyProductRecord.ProductName = request.ProductName;
                    isThereAnyProductRecord.Color = request.Color;
                    isThereAnyProductRecord.Size = request.Size;

                    _productRepository.Update(isThereAnyProductRecord);
                    await _productRepository.SaveChangesAsync();
                    return new SuccessResult(Messages.Updated);
                }
            }
        }
    }
}