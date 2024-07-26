using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class OrderRepository : EfEntityRepositoryBase<Order, ProjectDbContext>, IOrderRepository
    {
        public OrderRepository(ProjectDbContext context) : base(context)
        {
        }

        async Task<IEnumerable<OrderDto>> IOrderRepository.GetOrderDtoQuery()
        {
            return await (from o in Context.Orders
                              //join u in Context.Customers on o.CreatedUserId equals u.CreatedUserId
                              //join p in Context.Products on o.StorageId equals p.Id
                          join c in Context.Customers on o.CustomerId equals c.Id into oc
                          from c in oc.DefaultIfEmpty()
                          join p in Context.Products on o.StorageId equals p.Id into op
                          from p in op.DefaultIfEmpty()
                          where o.isDeleted == false
                          select new OrderDto
                          {
                              ProductId = p.Id,
                              Quantity = o.Quantity,
                              CustomerId = c.Id,
                              //Quantity = o.Quantity,
                              //ProductId = p.Id,
                              //StorageId =o.Id,
                              //CustomerId=u.Id,
                              //CustomerId=1

                          }
             ).ToListAsync();

        }
    }
}
