using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP1.Views
{
    public interface IView
    {
        object GetModel();
        void Init(object model);
    }
}
