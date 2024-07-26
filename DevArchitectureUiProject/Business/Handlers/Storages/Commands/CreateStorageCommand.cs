
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
using Business.Handlers.Storages.ValidationRules;

namespace Business.Handlers.Storages.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateStorageCommand :Storage, IRequest<IResult>
    {

        public class CreateStorageCommandHandler : IRequestHandler<CreateStorageCommand, IResult>
        {
            private readonly IStorageRepository _storageRepository;
            private readonly IMediator _mediator;
            public CreateStorageCommandHandler(IStorageRepository storageRepository, IMediator mediator)
            {
                _storageRepository = storageRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateStorageValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateStorageCommand request, CancellationToken cancellationToken)
            {
                //var isThereStorageRecord = _storageRepository.Query().Any(u => u.CreatedUserId == request.CreatedUserId);

                //if (isThereStorageRecord == true)
                //    return new ErrorResult(Messages.NameAlreadyExist);

                var addedStorage = new Storage
                {
                    CreatedUserId = request.CreatedUserId,
                    CreatedDate = System.DateTime.Now,
                    Status = true,
                    isDeleted = false,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    isReady = request.isReady,

                };

                _storageRepository.Add(addedStorage);
                await _storageRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}