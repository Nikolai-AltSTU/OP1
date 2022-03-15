using OP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP1.Views
{
    public class ProdCalcView : ProdCalc
    {
        public ProdCalcView() { }
        public ProdCalcView(ProdCalc prodCalc)
        {
            this.ProductPk = prodCalc.ProductPk;
            this.CalcPk = prodCalc.CalcPk;  
            this.ProdCalsPk = prodCalc.ProdCalsPk;
            this.CalcPkNavigation = prodCalc.CalcPkNavigation;  
            this.ProductPkNavigation = prodCalc.ProductPkNavigation;    
            this.Price = prodCalc.Price;
            this.Norma = prodCalc.Norma;
            this.Summa = prodCalc.Summa;
        }

        public double SummaDouble()
        {
            Summa = (long)(Norma * Price);
            return (double)(Norma * Price / 100);
        }

    }
}
