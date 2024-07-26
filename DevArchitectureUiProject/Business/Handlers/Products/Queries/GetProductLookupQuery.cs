using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Dtos;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.Products.Queries
{
    public class GetProductLookupQuery : IRequest<IDataResult<IEnumerable<SelectionItem>>>
    {
        public class GetProductLookupQueryHandler : IRequestHandler<GetProductLookupQuery, IDataResult<IEnumerable<SelectionItem>>>
        {
            private readonly IProductRepository _productRepository;

            public GetProductLookupQueryHandler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            [SecuredOperation(Priority = 1)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<IEnumerable<SelectionItem>>> Handle(GetProductLookupQuery request, CancellationToken cancellationToken)
            {
                var list = await _productRepository.GetListAsync(x => x.Status);
                var productLookup = list.Select(x => new SelectionItem() { Id = x.Id.ToString(), Label = x.ProductName });
                return new SuccessDataResult<IEnumerable<SelectionItem>>(productLookup);
            }
        }
    }
}