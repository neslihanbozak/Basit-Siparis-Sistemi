using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Storage : Common.BaseEntity, IEntity
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public bool isReady { get; set; }
    }
}
