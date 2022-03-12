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
        public string Okud { get; set; }
        public string Okpo { get; set; }
        public string Okdp { get; set; }
        public string DishNumber { get; set; }
        public string OperName { get; set; }
        public string DocNumber { get; set; }
        public DateTime? DateOfDoc { get ; set; }

        //public DateTime DateTimeOfDoc { get { return DateTime.Parse(System.Text.Encoding.ASCII.GetString(DateOfDoc));} set { DateOfDoc = value.ToString().ToCharArray(); } }

        public virtual ICollection<Calculation> Calculations { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
