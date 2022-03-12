using System;
using System.Collections.Generic;

namespace OP1.Models
{
    public partial class Product
    {
        public long ProductPk { get; set; }
        public string NameProd { get; set; }
        public long? Code { get; set; }
        public long? Number { get; set; }
        public long CardPk { get; set; }

        public virtual Card CardPkNavigation { get; set; }
    }
}
