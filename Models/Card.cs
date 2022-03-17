using System;
using System.Collections.Generic;

namespace OP1.Models
{
    public partial class Card
    {
        public Card()
        {
            Calculations = new HashSet<Calculation>();
            Products = new HashSet<Product>();
        }

        public long CardPk { get; set; }
        public string OrganizationName { get; set; }
        public string SubOrganization { get; set; }
        public string DishName { get; set; }
        public string Okud { get; set; } = "0330501";
        public string Okpo { get; set; }
        public string Okdp { get; set; }
        public string DishNumber { get; set; }
        public string OperName { get; set; }
        public string DocNumber { get; set; }
        public DateTime DateOfDoc { get; set; } = DateTime.Now;

        public virtual ICollection<Calculation> Calculations { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
