﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Product: Common.BaseEntity, IEntity
    {
       
        public string ProductName { get; set; }
        public int Color { get; set; }
        public int Size { get; set; }
    }

}
