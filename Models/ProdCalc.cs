using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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


        [NotMapped]
        public double PriceDouble
        {
            get { return (double)(double?)(Price / 100.0); }
            set { Price = (long?)Math.Round(value * 100); }
        }

        [NotMapped]
        public double SummaDouble
        {
            get { 
            Summa = (long) (Norma* Price);
            return (double) (Norma* Price / 100);
            }
        }

        public virtual Calculation CalcPkNavigation { get; set; }
        public virtual Product ProductPkNavigation { get; set; }
    }
}
