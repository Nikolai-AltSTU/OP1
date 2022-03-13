using OP1.Forms;
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


//  Scaffold-DbContext "Data Source=D:\PI-81\8_semester\HM_interfaces\Labs\Lab4\OP1\Database\OP1_v1.3.db;" Microsoft.EntityFrameworkCore.Sqlite -outputdir D:\PI-81\8_semester\HM_interfaces\Labs\Lab4\OP1\Models;

namespace OP1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        protected OP1_v13Context op1Context;
        protected Card card;
        protected ObservableCollection<ProductView> productViews;
        protected ObservableCollection<ProdCalc> prodCalcs = new();
        protected List<CalculationButton> CalculationButtons = new();
        protected CalculationButton selectedCalculationButton;

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
            if (op1Context.Cards.ToList().Count == 0)
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
            InitCalculations();
        }

        private void InitCalculations()
        {
            CalculationButtons.Clear();
            op1Context.Entry(card).Collection(card => card.Calculations).Load();
            foreach (var cals in card.Calculations.OrderBy(q => q.NumberCalc))
                CalculationButtons.Add(CreateQuestionButton(cals));
            if (card.Calculations.Count > 0)
                selectCalculation(CalculationButtons.First());
            RefreshButtonsWrapPanel();
        }

        private CalculationButton CreateQuestionButton(Calculation cals)
        {
            var btn = new CalculationButton { calculation = cals };
            btn.Click += CalculationBtnOnClick;
            UnselectColorBtn(btn);
            return btn;
        }

        private void selectCalculation(CalculationButton calculationButton)
        {
            if (selectedCalculationButton != null)
                    UnselectColorBtn(selectedCalculationButton);
            selectedCalculationButton = calculationButton;
            SelectColorBtn(selectedCalculationButton);
            prodCalcs.Clear();
            op1Context.Entry(selectedCalculationButton.calculation).Collection(calc => calc.ProdCalcs).Load();
            foreach(var cal in selectedCalculationButton.calculation.ProdCalcs)
            {
                prodCalcs.Add(cal);
            }
            prodCalcs.OrderBy(pc => pc.ProductPkNavigation.Number);
            CalculationsDataGrid.ItemsSource = prodCalcs;
            CalculationResultsBorder.DataContext = selectedCalculationButton.calculation;
        }

        private void CalculationBtnOnClick(object sender, RoutedEventArgs e)
        {
            selectCalculation((CalculationButton)sender);
        }

        private void UnselectColorBtn(CalculationButton btn) => btn.Style = FindResource("NavigationButtons") as Style;
        private void SelectColorBtn(CalculationButton btn) => btn.Style = FindResource("NavigationButtonsActive") as Style;


        private void RefreshButtonsWrapPanel()
        {
            ButtonsWrapPanel.Children.Clear();
            for (var i = 0; i < card.Calculations.Count; i++)
            {
                CalculationButtons[i].Click += CalculationBtnOnClick;
                CalculationButtons[i].calculation = card.Calculations.ElementAt(i);
                CalculationButtons[i].OrderNumber = i + 1;
                ButtonsWrapPanel.Children.Add(CalculationButtons[i]);
            }
            ButtonsWrapPanel.Visibility = card.Calculations.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        }


        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            op1Context.SaveChanges();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            op1Context.Products.Add(product);
            product.CardPk = card.CardPk;
            card.Products.Add(product);
            op1Context.SaveChanges();

            productViews.Add(new());
            product.Number = productViews.Count;
            productViews.Last().Init(product);

            foreach(var calc in card.Calculations)
            {
                op1Context.Entry(calc).Collection(calc => calc.ProdCalcs).Load();
                ProdCalc prodCalc = new();
                op1Context.Add(prodCalc);
                prodCalc.ProductPk = product.ProductPk;
                calc.ProdCalcs.Add(prodCalc);
            }
            if(selectedCalculationButton != null)
                selectCalculation(selectedCalculationButton);

        }

        private void RemProduct_Click(object sender, RoutedEventArgs e)
        {
            if(ProductsDataGrid.SelectedItems.Count > 1 || ProductsDataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите один продукт", "Не удалось удалить");
                return;
            };
            ProductView productView = (ProductView)ProductsDataGrid.SelectedItem;
            Product productToRemove = op1Context.Products.Find(
                ((Product)(productView).GetModel()).ProductPk);
            op1Context.Entry(productToRemove).Collection(product => product.ProdCalcs).Load();

            if(productToRemove.ProdCalcs.Count > 0)
            {
                MessageBoxResult messageBoxResult= MessageBox.Show("Удалить вместе со всеми калькуляциями?", "Удаление", MessageBoxButton.OKCancel);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    foreach (ProdCalc prodCalc in productToRemove.ProdCalcs)
                    {
                        prodCalc.CalcPkNavigation.ProdCalcs.Remove(prodCalc);
                        op1Context.Remove(prodCalc);
                    }
                }
                else
                    return;
            }
            int number = (int)productToRemove.Number;
            card.Products.Remove(productToRemove);
            productViews.Remove(productView);
            op1Context.Remove(productToRemove);

            foreach(ProductView productV in productViews)
            {
                if ( ((Product)productV.GetModel()).Number > number)
                    ((Product)productV.GetModel()).Number = ((Product)productV.GetModel()).Number - 1;
            }

            if (selectedCalculationButton != null)
                selectCalculation(selectedCalculationButton);

            ProductsDataGrid.ItemsSource = null;
            ProductsDataGrid.ItemsSource = productViews;
        }

        private void AddCalcButton_Click(object sender, RoutedEventArgs e)
        {
            Calculation calculation = new();
            op1Context.Calculations.Add(calculation);
            card.Calculations.Add(calculation);
            foreach (Product product in card.Products)
            {
                ProdCalc prodCalc = new ProdCalc();
                op1Context.Add(prodCalc);
                product.ProdCalcs.Add(prodCalc);
                prodCalc.ProductPk = product.ProductPk;
                calculation.ProdCalcs.Add(prodCalc);
            }

            CalculationButtons.Add(new());            
            RefreshButtonsWrapPanel();

            selectCalculation(CalculationButtons.Last());
        }
        private void RemCalcButton_Click(object sender, RoutedEventArgs e)
        {
            if(selectedCalculationButton == null)
            {
                MessageBox.Show("Калькуляции не выбрана", "Не удалось удалить");
                return;
            }

            MessageBoxResult messageBoxResult = MessageBox.Show(String.Format("Удалить калькуляцию №{0}?", selectedCalculationButton.calculation.NumberCalc) , "Удаление", MessageBoxButton.OKCancel);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                long number = (long)selectedCalculationButton.calculation.NumberCalc;
                foreach(ProdCalc prodCalc in selectedCalculationButton.calculation.ProdCalcs)
                    op1Context.Remove(prodCalc);
                selectedCalculationButton.calculation.ProdCalcs.Clear();
                card.Calculations.Remove(selectedCalculationButton.calculation);
                op1Context.Remove(selectedCalculationButton.calculation);
                foreach(Calculation calculation in card.Calculations)
                {
                    if (calculation.NumberCalc > number)
                        calculation.NumberCalc--;
                }
                CalculationButtons.RemoveAt((int)number-1);
                if (number > 0)
                    selectCalculation(CalculationButtons[(int)Math.Max(number-2, 0)]);
                else
                    selectedCalculationButton = null;
                RefreshButtonsWrapPanel();
            }
        }
    }
}
