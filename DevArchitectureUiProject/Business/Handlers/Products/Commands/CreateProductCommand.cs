﻿using Business.BusinessAspects;
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
using Business.Handlers.Products.ValidationRules;
using System.Runtime.Serialization;
using Nest;

namespace Business.Handlers.Products.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateProductCommand: Product, MediatR.IRequest<IResult>
    {


        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, IResult>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMediator _mediator;
            public CreateProductCommandHandler(IProductRepository productRepository, IMediator mediator)
            {
                _productRepository = productRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateProductValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                var isThereProductRecord = _productRepository.Query().Any(u => 
                                                                                u.ProductName == request.ProductName &&
                                                                                u.Color == request.Color &&
                                                                                u.Size == request.Size &&
                                                                                !u.isDeleted
                );

                if (isThereProductRecord)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedProduct = new Product
                {
                    CreatedUserId = request.CreatedUserId,
                    CreatedDate=System.DateTime.Now,                   
                    Status = request.Status,
                    isDeleted = false,
                    ProductName = request.ProductName,
                    Color = request.Color,
                    Size = request.Size,

                };

                _productRepository.Add(addedProduct);
                await _productRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}