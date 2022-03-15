using Microsoft.Win32;
using OP1.Forms;
using OP1.Models;
using OP1.Views;

//using Syncfusion.UI.Xaml.Grid.Converter;

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
using Syncfusion.XlsIO;
using System.Drawing;
using System.IO;


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

        #region loading
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
            {
                op1Context.Entry(cals).Collection(cals => cals.ProdCalcs).Load();
                CalculationButtons.Add(CreateQuestionButton(cals));
            }
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
        #endregion

        #region updateTriggers
        public void updateCalculationContext(object sender, DataGridCellEditEndingEventArgs e)
        {
            reassignContext();
        }
       
        public void updateCalculationContext(object sender, SelectedCellsChangedEventArgs e)
        {
            reassignContext();
        }
        private void updateCalculationContext(object sender, EventArgs e)
        { 
            // обрабатывает изменение данных в таблице
            reassignContext(); 
        }

        private void reassignContext()
        {
            var s = ExtraChargePercentTextBox.Text;
            CalculationsDataGrid.ItemsSource = null;
            CalculationResultsBorder.DataContext = null;
            CalculationsDataGrid.ItemsSource = prodCalcs;
            CalculationResultsBorder.DataContext = selectedCalculationButton.calculation;
            ExtraChargePercentTextBox.Text = s;
        }

        private void updateCalculationPrice(object sender, TextChangedEventArgs e)
        {
            updateCalculationPrice();
        }

        private void updateCalculationPrice()
        {
            double persent = 0;
            double.TryParse( ExtraChargePercentTextBox.Text, out persent);
            selectedCalculationButton.calculation.ExtraChargePercent = persent;
            ExtraChargeMoneyTextBox.Text = selectedCalculationButton.calculation.ExtraChargeMoneyView.ToString();
            SellingPriceTextBox.Text = selectedCalculationButton.calculation.SellingPrice.ToString();
            ExtraChargeMoneyTextBox.Text = ExtraChargeMoneyTextBox.Text;
        }

        #endregion

        #region userActivity
        private void selectCalculation(CalculationButton calculationButton)
        {
            if (selectedCalculationButton != null)
                    UnselectColorBtn(selectedCalculationButton);
            selectedCalculationButton = calculationButton;
            SelectColorBtn(selectedCalculationButton);
            prodCalcs.Clear();
            //op1Context.Entry(selectedCalculationButton.calculation).Collection(calc => calc.ProdCalcs).Load();
            foreach(ProdCalc cal in selectedCalculationButton.calculation.ProdCalcs)
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            loadDataIntoForm();
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
            op1Context.SaveChanges();
            if (selectedCalculationButton != null)
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

            if(card.Calculations.Count > 5)
            {
                AddCalcButton.Visibility = Visibility.Collapsed;
            }
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

            if (card.Calculations.Count < 6)
            {
                AddCalcButton.Visibility = Visibility.Visible;
            }
        }

        private void Signes_Click(object sender, RoutedEventArgs e)
        {
            Signs signs = new Signs() { DataContext = new Calculation()
                {
                    Zaveduushiy = selectedCalculationButton.calculation.Zaveduushiy,
                    Sostavitel = selectedCalculationButton.calculation.Sostavitel,
                    Rukovoditel = selectedCalculationButton.calculation.Rukovoditel
                } 
            };
            signs.ShowDialog();
            if(signs.DialogResult == true)
            {
                selectedCalculationButton.calculation.Zaveduushiy = ((Calculation)signs.DataContext).Zaveduushiy;
                selectedCalculationButton.calculation.Sostavitel = ((Calculation)signs.DataContext).Sostavitel;
                selectedCalculationButton.calculation.Rukovoditel = ((Calculation)signs.DataContext).Rukovoditel;
            }
        }


        #endregion

        #region printing
        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "Excel document (*.xlsx, *.xls)|*.xlsx;*.xls";
            saveFileDialog.Title = "Выберите название документа для сохранения";

            if(saveFileDialog.ShowDialog() == true)
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2016;

                    IWorkbook workbook = application.Workbooks.Open("xlsx\\LAW_26677.attach_LAW_26677_1.XLS").Clone();
                    IWorksheet worksheet = workbook.Worksheets[0];

                    // header
                    worksheet.Range["BC5"].Text = card.Okud;
                    worksheet.Range["BC6"].Text = card.Okpo;
                    worksheet.Range["A6"].Text = card.OrganizationName;
                    worksheet.Range["A8"].Text = card.SubOrganization;
                    worksheet.Range["A10"].Text = card.DishName;
                    worksheet.Range["BC9"].Text = card.Okdp;
                    worksheet.Range["BC11"].Text = card.DishNumber;
                    worksheet.Range["BC12"].Text = card.OperName;

                    // номер карточки
                    worksheet.Range["AD14"].Text = card.DocNumber;
                    worksheet.Range["AL14"].Text = card.DateOfDoc != null ? card.DateOfDoc.ToString("dd.MM.yyyy") : "";

                    //worksheet.Range["A33", "BA40"].CopyTo(worksheet.Range["A40", "BA47"]);

                    // продукты
                    const int start_line = 22;
                    int row_index = start_line;

                    const int max_default_number_of_products = 11;

                    int row_index_summ = 32;

                    if (card.Products.Count > max_default_number_of_products)
                    {
                        worksheet.InsertRow(row_index_summ, card.Products.Count - max_default_number_of_products);
                        for (int i = 0; i < card.Products.Count - max_default_number_of_products; i++)
                        {
                            worksheet.Rows[27].CopyTo(worksheet.Rows[row_index_summ + i - 1]);
                            worksheet.Rows[row_index_summ + i - 1].RowHeight = worksheet.Rows[27].RowHeight;
                        }
                        row_index_summ += card.Products.Count - max_default_number_of_products;
                    }

                    foreach (Product product in card.Products.OrderBy(prod => prod.Number))
                    {
                        worksheet.Range[string.Format("A{0}", row_index)].Text = product.Number.ToString();
                        worksheet.Range[string.Format("C{0}", row_index)].Text = product.NameProd;
                        worksheet.Range[string.Format("H{0}", row_index)].Text = product.Code.ToString();
                        row_index++;
                    }

                    // калькуляции
                    const int col_index_start = 9;
                    const int col_index_step = 8;

                    int col_index = col_index_start;

                    const int calcHeaderIndex = 16;

                    foreach (Calculation calculation in card.Calculations.OrderBy(calc => calc.NumberCalc))
                    {                       
                        worksheet.Rows[calcHeaderIndex].Cells[col_index_start + 2].Text = calculation.DateCalc.Day.ToString();
                        worksheet.Rows[calcHeaderIndex].Cells[col_index_start + 4].Text = calculation.DateCalc.Month.ToString();
                        worksheet.Rows[calcHeaderIndex].Cells[col_index_start + 6].Text = calculation.DateCalc.Year.ToString();

                        row_index = start_line-1;
                        
                        foreach (ProdCalc prodCalc in calculation.ProdCalcs)  // OrderBy(prodCalc => card.Products.Where(prod => prod.ProdCalcs.Contains(prodCalc)) )
                        {
                            ProdCalcView prodCalcView = new ProdCalcView(prodCalc);
                            //worksheet.Rows[0].Cells[0].Text = "asdasd";

                            worksheet.Rows[row_index].Cells[col_index].Text = prodCalcView.Norma.ToString();
                            worksheet.Rows[row_index].Cells[col_index + 3].Text = prodCalcView.PriceDouble.ToString();
                            worksheet.Rows[row_index].Cells[col_index + 5].Text = prodCalcView.SummaDouble().ToString();
                            row_index++;
                        }
                        
                        // итого, подписи           
                                                                //14
                        worksheet.Rows[row_index_summ].Cells[col_index + 5].Text = calculation.AllCostsPer100Dishes.ToString();
                        worksheet.Rows[row_index_summ + 1].Cells[col_index].Text = calculation.ExtraChargePercent.ToString() + ", " + calculation.ExtraChargeMoneyView.ToString(); ///????
                        worksheet.Rows[row_index_summ + 3].Cells[col_index].Text = calculation.SellingPrice.ToString();
                        worksheet.Rows[row_index_summ + 4].Cells[col_index].Text = calculation.DishWeihtView.ToString();

                        worksheet.Rows[row_index_summ + 5].Cells[col_index].Text = calculation.Zaveduushiy;
                        worksheet.Rows[row_index_summ + 6].Cells[col_index].Text = calculation.Sostavitel;
                        worksheet.Rows[row_index_summ + 7].Cells[col_index].Text = calculation.Rukovoditel;

                        col_index += col_index_step;
                    }
                    workbook.SaveAs(saveFileDialog.FileName);
                }
            }
        }

        #endregion

    }
}
