﻿using System;
using System.Collections.Generic;

namespace OP1.Models
{
    public partial class ProdCalc
    {
        public double? Norma { get; set; }
        public long? Price { get; set; }
        public long? Summa { get; set; }
        public long CalcFpk { get; set; }
        public long ProductPk { get; set; }
        public long? CardPk { get; set; }
    }
}
