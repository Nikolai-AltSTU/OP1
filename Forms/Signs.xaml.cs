using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OP1.Forms
{
    /// <summary>
    /// Логика взаимодействия для Signs.xaml
    /// </summary>
    public partial class Signs : Window
    {
        public Signs()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cansel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
