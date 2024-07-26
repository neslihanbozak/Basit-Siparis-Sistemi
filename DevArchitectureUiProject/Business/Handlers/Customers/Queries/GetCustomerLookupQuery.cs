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


namespace Business.Handlers.Customers.Queries
{
    public class GetCustomerLookupQuery : IRequest<IDataResult<IEnumerable<SelectionItem>>>
    {
        public class GetCustomerLookupQueryHandler : IRequestHandler<GetCustomerLookupQuery, IDataResult<IEnumerable<SelectionItem>>>
        {
            private readonly ICustomerRepository _customerRepository;

            public GetCustomerLookupQueryHandler(ICustomerRepository customerRepository)
            {
                _customerRepository = customerRepository;
            }

            [SecuredOperation(Priority = 1)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<IEnumerable<SelectionItem>>> Handle(GetCustomerLookupQuery request, CancellationToken cancellationToken)
            {
                var list = await _customerRepository.GetListAsync(x => x.Status);
                var customerLookup = list.Select(x => new SelectionItem() { Id = x.Id.ToString(), Label = x.CustomerName});
                return new SuccessDataResult<IEnumerable<SelectionItem>>(customerLookup);
            }
        }
    }
}