using OP1.Models;
using OP1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OP1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        protected OP1Context op1Context;
        protected Card card;
        protected ObservableCollection<ProductView> productViews;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            loadDataIntoForm();
        }

        private void loadDataIntoForm()
        {
            op1Context = new();
            if(op1Context.Cards.Count() == 0)
            {
                card = new();
                op1Context.Cards.Add(card); 
                op1Context.SaveChanges();   
            }
            else
            {
                card = op1Context.Cards.First();
                op1Context.Entry(card).Collection(c => c.Products).Load();
            }
            uploadData();
        }

        private void uploadData()
        {
            CardBorder.DataContext = card;
            HeadingStack.DataContext = card;

            productViews = new();
            foreach (Product product in card.Products)
            {
                productViews.Add(new());
                productViews.Last().Init(product);
            }
            ProductsDataGrid.ItemsSource = productViews;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            op1Context.SaveChanges();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            product.CardPk = card.CardPk;
            op1Context.Products.Add(product);
            productViews.Add(new());
            productViews.Last().Init(product);
        }

        private void RemProduct_Click(object sender, RoutedEventArgs e)
        {
            if(ProductsDataGrid.SelectedItems.Count > 1 || ProductsDataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите один продукт", "Не удалось удалить");
                return;
            };
            ProductView productView = (ProductView)ProductsDataGrid.SelectedItem;
            Product productToRemove = op1Context.Products.Find( ((Product)(productView).GetModel()).ProductPk,
                 ((Product)(productView).GetModel()).CardPk );
            op1Context.Entry(productToRemove).Collection(product => product.ProdCalcs).Load();

            if(productToRemove.ProdCalcs.Count > 0)
            {
                MessageBoxResult messageBoxResult= MessageBox.Show("Удалить вместе со всеми калькуляциями?", "Удаление", MessageBoxButton.OKCancel);
                if(messageBoxResult == MessageBoxResult.OK)
                {
                    op1Context.Remove(productToRemove.ProdCalcs);
                    productViews.Remove(productView);
                    op1Context.Remove(productToRemove);
                }
            }
            else
            {
                productViews.Remove(productView);
                op1Context.Remove(productToRemove);
            }

            op1Context.SaveChanges();
        }
    }
}
