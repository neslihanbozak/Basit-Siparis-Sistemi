using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Order : Common.BaseEntity,IEntity
    {
        public int CustomerId { get; set; }
        public int StorageId { get; set; }
        public int Quantity { get; set; }
       
    }
}
