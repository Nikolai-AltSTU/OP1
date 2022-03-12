using OP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP1.Views
{
    public partial class ProductView : IView
    {
        private Product product;
        public object GetModel()
        {   
            return product;
        }

        public void Init(object model)
        {
            product = (Product)model;
        }

        public string Name { get { return product.NameProd; } set { product.NameProd = value; } }

        public long Code { get { return (long)product.Code; } set { product.Code = value; } }
        

    }
}
