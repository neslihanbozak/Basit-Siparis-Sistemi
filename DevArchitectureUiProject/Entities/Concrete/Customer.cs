using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Customer : Common.BaseEntity, IEntity
    {
        public string CustomerName { get; set; }
        public int CustomerCode { get; set; }
        public string Address { get; set; }
        public string CustomerPhone { get; set; }
        public string Email { get; set; }


    }
}
