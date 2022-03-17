using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime DateCalc { get; set; } = DateTime.Now;
        public double? DishWeght { get; set; }
        public double? ExtraChargePercent { get; set; }
        public long? ExtraChargeMoney { get; set; }
        public string Zaveduushiy { get; set; }
        public string Sostavitel { get; set; }
        public string Rukovoditel { get; set; }
        public long CardPk { get; set; }


        [NotMapped]
        public double? SellingPrice
        {
            get
            {
                return ((double)AllCostsPer100Dishes * (1 + ExtraChargePercent / 100) / 100);
            }
            set { }
        }

        [NotMapped]
        public double? ExtraChargeMoneyView { 
            get {
                return (ExtraChargeMoney = ((long?)(AllCostsPer100Dishes * ExtraChargePercent / 100)) / 100);
            } 
            set { } 
        }  

        [NotMapped]
        public double AllCostsPer100Dishes{
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
        }

        [NotMapped]
        public double? DishWeihtView
        {
            get
            {
                if (DishWeght.HasValue == false || DishWeght == 0)
                {
                    DishWeght = 0;
                    foreach (ProdCalc prodCalc in ProdCalcs)
                    {
                        DishWeght += prodCalc.Norma * 1000;
                    }
                }
                return DishWeght;
            }
            set { DishWeght = value; }
        }

        public virtual Card CardPkNavigation { get; set; }
        public virtual ICollection<ProdCalc> ProdCalcs { get; set; }
    }
}
