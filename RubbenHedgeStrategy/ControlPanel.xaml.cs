using ATAS.DataFeedsCore;
using ATAS.Strategies;
using ATAS.Strategies.Chart;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace RubbenHedgeStrategy
{
    /// <summary>
    /// Interaction logic for ControlPanel.xaml
    /// </summary>
    /// 

    public partial class ControlPanel : Window
    {
        private readonly RubbenHedge _rubbenHedge;

        public ControlModels ControlModels { get; set; }
        public ControlPanel(RubbenHedge rubbenHedge)
        {

            this.InitializeComponent();

            _rubbenHedge = rubbenHedge;
            this.ControlModels = new ControlModels();
            this.DataContext = this.ControlModels;
            
            //GlobalVariablesTemp.IsShowControlPanel = true;
        }

        public void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (GlobalVariables.IsAlreadyOpenedControlPanel) return;
            GlobalVariables.IsAlreadyOpenedControlPanel = true;
           
        }

        public void ControlPanel_OnClosed(object sender, EventArgs e)
        {
            GlobalVariables.IsAlreadyOpenedControlPanel = false;
        }
         

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ControlModels.IsStrategyActive)
            {
                
                StartButton.Content = "Stop";
            }
            else
            {
                //GlobalVariables.isActiveOrder1 = false;
                //GlobalVariables.isActiveOrder2 = false;
                //GlobalVariables.isActiveOrder3 = false;
                //GlobalVariables.isActiveOrder4 = false;
                StartButton.Content = "Start";
                _rubbenHedge.ActiveStop();
                //_rubbenHedge.CloseCurrentPosition();
                //_rubbenHedge.CloseCurrentOrders();
            }
            ControlModels.IsStrategyActive = !ControlModels.IsStrategyActive;
        }

        private void Reset_1()
        {
            lock (ControlModels)
            {
                ControlModels.Price1 = decimal.TryParse(Text_Price1.Text, out decimal result) ? result : 0m;
                ControlModels.SelectedSideItem1 = Combo_Side1.SelectedValue.ToString();
                ControlModels.SelectedTypeItem1 = Combo_Type1.SelectedValue.ToString();
                ControlModels.SelectedCrossItem1 = Combo_Cross1.SelectedValue.ToString();
                ControlModels.Stoploss1 = decimal.TryParse(Text_SL1.Text, out decimal result1) ? result1 : 0;
                ControlModels.BreakEven1 = decimal.TryParse(Text_BE1.Text, out decimal result2) ? result2 : 0;
                ControlModels.Amount1 = decimal.TryParse(Text_Amount1.Text, out decimal result3) ? result3 : 0;
                ControlModels.CloseDate1 = Text_Date1.Text;
                ControlModels.CloseTime1 = Text_Time1.Text;
                MessageBox.Show("Reset is done successfully.");
            }
        }
        private void Reset_2()
        {
            lock (ControlModels)
            {
                ControlModels.Price2 = decimal.TryParse(Text_Price2.Text, out decimal result) ? result : 0m;
                ControlModels.SelectedSideItem2 = Combo_Side2.SelectedValue.ToString();
                ControlModels.SelectedTypeItem2 = Combo_Type2.SelectedValue.ToString();
                ControlModels.SelectedCrossItem2 = Combo_Cross2.SelectedValue.ToString();
                ControlModels.Stoploss2 = decimal.TryParse(Text_SL2.Text, out decimal result1) ? result1 : 0;
                ControlModels.BreakEven2 = decimal.TryParse(Text_BE2.Text, out decimal result2) ? result2 : 0;
                ControlModels.Amount2 = decimal.TryParse(Text_Amount2.Text, out decimal result3) ? result3 : 0;
                ControlModels.CloseDate2 = Text_Date2.Text;
                ControlModels.CloseTime2 = Text_Time2.Text;
                MessageBox.Show("Reset is done successfully.");
            }
        }

        //private void Reset_3()
        //{
        //    lock (ControlModels)
        //    {
        //        ControlModels.Price3 = decimal.TryParse(Text_Price3.Text, out decimal result) ? result : 0m;
        //        ControlModels.SelectedSideItem3 = Combo_Side3.SelectedValue.ToString();
        //        ControlModels.SelectedTypeItem3 = Combo_Type3.SelectedValue.ToString();
        //        ControlModels.SelectedCrossItem3 = Combo_Cross3.SelectedValue.ToString();
        //        ControlModels.Stoploss3 = decimal.TryParse(Text_SL3.Text, out decimal result1) ? result1 : 0;
        //        ControlModels.BreakEven3 = decimal.TryParse(Text_BE3.Text, out decimal result2) ? result2 : 0;
        //        ControlModels.Amount3 = decimal.TryParse(Text_Amount3.Text, out decimal result3) ? result3 : 0;
        //        ControlModels.CloseDate3 = Text_Date3.Text;
        //        ControlModels.CloseTime3 = Text_Time3.Text;
        //        MessageBox.Show("Reset is done successfully.");
        //    }
        //}

        //private void Reset_4()
        //{
        //    lock (ControlModels)
        //    {
        //        ControlModels.Price4 = decimal.TryParse(Text_Price4.Text, out decimal result) ? result : 0m;
        //        ControlModels.SelectedSideItem4 = Combo_Side4.SelectedValue.ToString();
        //        ControlModels.SelectedTypeItem4 = Combo_Type4.SelectedValue.ToString();
        //        ControlModels.SelectedCrossItem4 = Combo_Cross4.SelectedValue.ToString();
        //        ControlModels.Stoploss4 = decimal.TryParse(Text_SL4.Text, out decimal result1) ? result1 : 0;
        //        ControlModels.BreakEven4 = decimal.TryParse(Text_BE4.Text, out decimal result2) ? result2 : 0;
        //        ControlModels.Amount4 = decimal.TryParse(Text_Amount4.Text, out decimal result3) ? result3 : 0;
        //        ControlModels.CloseDate4 = Text_Date4.Text;
        //        ControlModels.CloseTime4 = Text_Time4.Text;
        //        MessageBox.Show("Reset is done successfully.");
        //    }
        //}
        private void Button_Reset1_Click(object sender, RoutedEventArgs e)
        {
            Reset_1();
        }

        private void Button_Reset2_Click(object sender, RoutedEventArgs e)
        {
            Reset_2();
        }

        //private void Button_Reset3_Click(object sender, RoutedEventArgs e)
        //{
        //    Reset_3();
        //}

        //private void Button_Reset4_Click(object sender, RoutedEventArgs e)
        //{
        //    Reset_4();
        //}


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {

            _rubbenHedge.CloseAllPosition();
        }
    }
}
