using System;
using System.Collections.Generic;

namespace OP1.Models
{
    public partial class ProdCalc
    {
        public double? Norma { get; set; }
        public byte[] Price { get; set; }
        public byte[] Summa { get; set; }
        public long CalcFpk { get; set; }
        public long CardFpk { get; set; }
        public long? ProductPk { get; set; }
        public long CardPk { get; set; }
    }
}
