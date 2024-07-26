
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
using System.Threading.Tasks;
using System.Collections.Generic;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class StorageRepository : EfEntityRepositoryBase<Storage, ProjectDbContext>, IStorageRepository
    {
        public StorageRepository(ProjectDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<StorageDto>> StorageDto()
        {
            var list = await (from storage in Context.Storages
                              join u in Context.Customers on storage.CreatedUserId equals u.CreatedUserId
                              join p in Context.Products on storage.ProductId equals p.Id
                              where storage.isDeleted == false
                              select new StorageDto
                              {
                                  Color = p.Color,
                                  CustomerName = u.CustomerName,
                                  ProductName = p.ProductName,
                                  Quantity = storage.Quantity,
                                  Size = p.Size
                              }
                              ).ToListAsync();

            return list;
        }
    }
}
