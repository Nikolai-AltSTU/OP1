using System;
using System.Collections.Generic;

namespace OP1.Models
{
    public partial class Calculation
    {
        public Calculation()
        {
            ProdCalcs = new HashSet<ProdCalc>();
        }

        public long CalcPk { get; set; }
        public long? NumberCalc { get; set; }
        public byte[] DateCalc { get; set; }
        public double? DishWeght { get; set; }
        public double? ExtraChargePercent { get; set; }
        public long? ExtraChargeMoney { get; set; }
        public string Zaveduushiy { get; set; }
        public string Sostavitel { get; set; }
        public string Rukovoditel { get; set; }
        public long CardPk { get; set; }

        public virtual Card CardPkNavigation { get; set; }
        public virtual ICollection<ProdCalc> ProdCalcs { get; set; }
    }
}
