using OP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP1.Views
{
    public class CalculationView : Calculation
    {
        protected Calculation calculation;
        public CalculationView(Calculation calculation)
        {
            this.calculation = calculation;
        }

        /*
            this.NumberCalc = calculation.NumberCalc;
            this.CalcPk = calculation.CalcPk;
            this.CardPk = calculation.CardPk;
            this.DateCalc = calculation.DateCalc;   
            this.ExtraChargePercent = calculation.ExtraChargePercent;   
            this.ExtraChargeMoney = calculation.ExtraChargeMoney;   
            this.CardPkNavigation = calculation.CardPkNavigation;
            this.DishWeght = calculation.DishWeght; 
            this.ProdCalcs = calculation.ProdCalcs;
            this.Rukovoditel = calculation.Rukovoditel;
            this.Sostavitel = calculation.Sostavitel;
            this.Zaveduushiy = calculation.Zaveduushiy; 
         */

        public double AllCostsPer100Dishes
        {
            get
            {
                double costs = 0;
                foreach (var pc in ProdCalcs)
                {
                    double? mult = pc.Price * pc.Norma;
                    if (mult.HasValue)
                        costs += (double)mult;
                }
                return costs;
            }
            set
            {
                AllCostsPer100Dishes = value;
            }
        }
    }
}
