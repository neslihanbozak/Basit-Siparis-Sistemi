
using Business.BusinessAspects;
using Core.Utilities.Results;
using Core.Aspects.Autofac.Performance;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Caching;
using Entities.Dtos;

namespace Business.Handlers.Storages.Queries
{

    public class GetStorageDtoQuery : IRequest<IDataResult<IEnumerable<StorageDto>>>
    {
        public class GetStorageDtoQueryHandler : IRequestHandler<GetStorageDtoQuery, IDataResult<IEnumerable<StorageDto>>>
        {
            private readonly IStorageRepository _storageRepository;
            private readonly IMediator _mediator;

            public GetStorageDtoQueryHandler(IStorageRepository storageRepository, IMediator mediator)
            {
                _storageRepository = storageRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<StorageDto>>> Handle(GetStorageDtoQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<StorageDto>>(await _storageRepository.StorageDto());
            }
        }
    }
}