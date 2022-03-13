using System;
using System.Collections.Generic;

namespace OP1.Models
{
    public partial class ProdCalc
    {
        public double? Norma { get; set; } = 0;
        public long? Price { get; set; } = 0;
        public long? Summa { get; set; } = 0;
        public long ProdCalsPk { get; set; }
        public long CalcPk { get; set; }
        public long? ProductPk { get; set; }

        public virtual Calculation CalcPkNavigation { get; set; }
        public virtual Product ProductPkNavigation { get; set; }
    }
}
