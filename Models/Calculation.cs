using System;
using System.Collections.Generic;

namespace OP1.Models
{
    public partial class Calculation
    {
        public long CalcPk { get; set; }
        public long? NumberCalc { get; set; }
        public byte[] DateCalc { get; set; }
        public double? DishWeght { get; set; }
        public double? ExtraChargePercent { get; set; }
        public byte[] ExtraChargeMoney { get; set; }
        public string Zaveduushiy { get; set; }
        public string Sostavitel { get; set; }
        public string Rukovoditel { get; set; }
        public long CardPk { get; set; }

        public virtual Card CardPkNavigation { get; set; }
        public virtual ProdCalc ProdCalc { get; set; }
    }
}
