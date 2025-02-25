
using ATAS.Strategies;
using ATAS.Indicators;
using ATAS.Strategies.Chart;
using System.ComponentModel;
using Utils.Common.Logging;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Security.RightsManagement;
using PhantomFlow_SFP.Template.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using System.Diagnostics;
using System.Globalization;
using ATAS.DataFeedsCore;
using ATAS.Strategies.ATM;
using System.Windows.Controls;
using System.Numerics;
using System.Windows.Media;
using ATAS.Indicators.Drawing;
using System.Windows.Shapes;
using System.Drawing;
using System.Security.Policy;
using System;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.DirectoryServices;
using System.Transactions;

namespace RubbenHedgeStrategy
{

    [DisplayName("RubbenHedgeStrategy")]
    public class RubbenHedge : ChartStrategy
    {
        public bool isShowControlPanel = false;
        ControlPanel? controlPanel = null;

        private decimal[] Amount= new decimal[4];
        private decimal[] Price = new decimal[4];
        private string[] Side = new string[4];
        private string[] Type = new string[4];
        private string[] Cross = new string[4];
        private decimal[] StopLoss=new decimal[4];
        private decimal[] BreakEven = new decimal[4];
        private string[] CloseDate = new string[4];
        private string[] CloseTime = new string[4];

        private bool isActivateOrder = false;
        //private bool[] isActivateOrder = new bool[4];

        private Order? MainOrder = null;
        private Order? SLOrder = null;

        private int currentConsideringID = -1; 
        private Dictionary<long, decimal> ExtIDToPrice = new();
        private long SLOrderID = -1;
        private long MainOrderID = -1;
        private decimal currentSL = -1;
        private decimal currentBE = -1;
        private decimal previousPrice = 0;
        private decimal currentPrice = 0;
        private decimal currentTriggerPrice = -1;


        public RubbenHedge()
        {
            currentBE = -1;
            currentSL = -1;
            previousPrice = 0;
            currentPrice = 0;
            currentTriggerPrice = -1;
            MainOrder = null;
            SLOrder = null;
            //GlobalVariablesTemp.isShowControlPanel= false;
        }

        public void ShowControlPanel()
        {
            if (!GlobalVariables.IsAlreadyOpenedControlPanel)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        controlPanel = new ControlPanel(this);
                        controlPanel.Show();
                        GlobalVariables.IsAlreadyOpenedControlPanel = true;
                    }
                    catch (Exception ex)
                    {
                        this.LogInfo($"ControlPanel show error : {ex.Message}");
                        //this.LoggingLevel.Error($"{ex}");
                    }
                });
            }
        }
        
        // Validate Date :  Example  02-13-2023 12:23:42
        private DateTime ValidateDate(string datetime)
        {
            DateTime parseDatetime;
            bool isValid = DateTime.TryParseExact(
                datetime,
                "MM-dd-yyyy HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out parseDatetime
                );
            if (isValid) return parseDatetime;
            else return DateTime.MinValue;
            
        }
        // Validate Time
          

        private void PlaceOrder(
            int index,
            decimal CurrentPrice,
            decimal PreviousPrice,
            decimal BestAskPrice,
            decimal BestBidPrice)
        {
            if (PreviousPrice == -10000) return; 
            if (Price[index] == 0 || Amount[index] == 0 || Side[index] == "" || Side[index] == null || Type[index] == "" || Type[index] == null || Cross[index] == "" || Cross[index] == null) return;
            DateTime dateTime = ValidateDate(CloseDate[index] + " " + CloseTime[index]);
            if (dateTime == DateTime.MinValue)
            {
                this.LogInfo("Here is DateTime"); return;
            }
            try
            {
                if ((Cross[index] == "Up" && PreviousPrice < Price[index] && Price[index] <= CurrentPrice) || (Cross[index] == "Down" && PreviousPrice > Price[index] && Price[index] >= CurrentPrice))
                {
                    Order order;
                    currentTriggerPrice = Type[index] == "Market" ? CurrentPrice : (Side[index] == "Buy" ? BestBidPrice : BestBidPrice);
                    decimal slPrice = Side[index] == "Buy" ? (currentTriggerPrice * (1 - StopLoss[index] / 100)) : (currentTriggerPrice * (1 + StopLoss[index] / 100));
                    currentConsideringID = index;
                    this.LogInfo($"Index: {index} slPrice:{slPrice} executePrice: {currentTriggerPrice} CurrentPrice:{currentTriggerPrice} BestBidPrice:{BestBidPrice} BestAskPrice:{BestAskPrice}");
                    //decimal = 
                    if (Type[index] == "Market")
                    {
                        order = new Order
                        {
                            QuantityToFill = Amount[index],
                            Type = OrderTypes.Market,
                            Portfolio = Portfolio,
                            Security = Security,
                            ExpiryDate = dateTime,
                            OCOGroup = $"Order_{Guid.NewGuid()}",
                            Direction = (Side[index] == "Buy" ? OrderDirections.Buy : OrderDirections.Sell),
                        };
                         OpenOrder(order);
                    }
                    else
                    {
                        order = new Order
                        {
                            QuantityToFill = Amount[index],
                            TriggerPrice = currentTriggerPrice,
                            Type = OrderTypes.Stop,
                            Portfolio = Portfolio,
                            Security = Security,
                            ExpiryDate = dateTime,
                            OCOGroup = $"Order_{Guid.NewGuid()}",
                            Direction = (Side[index] == "Buy" ? OrderDirections.Buy : OrderDirections.Sell),
                            //Price= price,

                        };
                        OpenOrder(order);
                    }
                    //stoporder = new Order
                    //{
                    //    QuantityToFill = order.QuantityToFill,
                    //    Direction = order.Direction == OrderDirections.Buy ? OrderDirections.Sell : OrderDirections.Buy,
                    //    Type = OrderTypes.Stop,
                    //    Portfolio = order.Portfolio,
                    //    Security = order.Security,
                    //    ExpiryDate = order.ExpiryDate,
                    //    OCOGroup = order.OCOGroup,
                    //    TriggerPrice = slPrice
                    //};
                    //await OpenOrderAsync(stoporder);
                } 

            }
            catch (Exception ex)
            {
                currentConsideringID = -1;
                this.LogInfo($"Here is place order Error {ex.Message}");
                return;
            }


        }

        private void TrySetReduceOnly(Order order)
        {
            var flags = TradingManager?
              .GetSecurityTradingOptions()?
              .CreateExtendedOptions(order.Type);

            if (flags is not IOrderOptionReduceOnly ro)
                return;

            ro.ReduceOnly = true;

            order.ExtendedOptions = flags;
        }
        protected override void OnCalculate(int bar, decimal value)
        {
            
            currentPrice  = value;  
            decimal BestAskPrice = this.BestAsk.Price;
            decimal BestBidPrice = this.BestBid.Price; 
            //this.LogInfo($"AskPrice: {BestAskPrice}  BidPrice:{BestBidPrice}  CurrentPrice : {currentPrice}");
            //return;
            if (controlPanel != null)
            {
                ControlModels controlModels = controlPanel.ControlModels;
                //MessageBox.Show($"{controlModels.IsStrategyActive}");
                if (controlModels.IsStrategyActive != true) return;
                this.LogInfo($"-----isActivateOrder: {isActivateOrder}  currentConsideringId: {currentConsideringID} CurrentPrice:{currentPrice}" );
                if (controlModels.Enable1 && isActivateOrder == false && currentConsideringID == -1)
                {
                    Amount[0] = controlModels.Amount1;
                    Price[0] = controlModels.Price1;
                    Side[0] = controlModels.SelectedSideItem1;
                    Type[0] = controlModels.SelectedTypeItem1;
                    Cross[0] = controlModels.SelectedCrossItem1;
                    StopLoss[0] = controlModels.Stoploss1;
                    BreakEven[0] = controlModels.BreakEven1;
                    CloseDate[0] = controlModels.CloseDate1;
                    CloseTime[0] = controlModels.CloseTime1; 
                    PlaceOrder(0, currentPrice, previousPrice, BestAskPrice, BestBidPrice);
                }
                if (controlModels.Enable2 && isActivateOrder == false && currentConsideringID == -1)
                {
                    Amount[1] = controlModels.Amount2;
                    Price[1] = controlModels.Price2;
                    Side[1] = controlModels.SelectedSideItem2;
                    Type[1] = controlModels.SelectedTypeItem2;
                    Cross[1] = controlModels.SelectedCrossItem2;
                    StopLoss[1] = controlModels.Stoploss2;
                    BreakEven[1] = controlModels.BreakEven2;
                    CloseDate[1] = controlModels.CloseDate2;
                    CloseTime[1] = controlModels.CloseTime2;
                    PlaceOrder(1, currentPrice, previousPrice, BestAskPrice, BestBidPrice);
                }
            }
            previousPrice = currentPrice;
        }

        public async void CloseCurrentOrders()
        {
            try
            {
                if (Orders.Count() != 0)
                    foreach (Order or in Orders)
                    {
                        if (or != null)
                        {
                            await CancelOrderAsync(or);
                        }
                    }
            }
            catch
            {

            }
        }
        public async void CloseCurrentPosition()
        {
            //MessageBox.Show("Hello This is CloseCurrentPosition");
            //this.LogError($"CurrentPostion: {CurrentPosition}");
            //if (GlobalVariables.isActiveOrder1 == false) return; 
            if (CurrentPosition != 0)
                try
                {
                    Order order = new Order
                    {
                        //ExtId =  100000
                        Portfolio = Portfolio,
                        Security = Security,
                        Direction = CurrentPosition > 0 ? OrderDirections.Sell : OrderDirections.Buy,
                        Type = OrderTypes.Market,
                        QuantityToFill = Math.Abs(CurrentPosition)
                    };
                    await OpenOrderAsync(order);
                    currentBE = -1;
                    currentSL = -1;

                }
                catch (Exception e)
                {
                    this.LogInfo($"CloseCurrentPosition Error: {e}");
                }
        }
        protected override void OnStarted()
        {
            MainOrderID = -1;
            SLOrderID = -1;
            currentSL = -1;
            currentBE = -1;
            currentTriggerPrice = -1;
            currentConsideringID = -1;
            //base.OnStarted();
            ShowControlPanel();
            previousPrice = -10000;
        }
        protected override void OnStopped()
        {
            //base.OnStopped();
            if (GlobalVariables.IsAlreadyOpenedControlPanel && controlPanel != null)
            {
                controlPanel.Close();
                GlobalVariables.IsAlreadyOpenedControlPanel = false;
            }
        }

        protected override void OnOrderRegisterFailed(Order order, string message)
        {
            //isActivateOrder = false;
            MessageBox.Show($"OrderIndex: {currentConsideringID}  Message:{message}");
            currentConsideringID = -1;
            //base.OnOrderRegisterFailed(order, message);
        }

        protected override void OnOrderChanged(Order order)
        {
            this.LogInfo($"####OrderStatus: {order.Status()} OrderState:{order.State}");
            //if (order.Status() == OrderStatus.Filled)
            //{
            //    this.LogInfo($"@@@@ MainOrderID: {order?.ExtId} OrderStatus: {order?.Status()}");

            //    if (MainOrder == null)
            //    {
            //        MainOrder = order?.Clone();
            //        isActivateOrder = true;
            //        currentBE = BreakEven[currentConsideringID];
            //        currentSL = StopLoss[currentConsideringID];
            //        decimal triggerPrice = order?.Direction == OrderDirections.Buy ? currentTriggerPrice * (1 - currentSL / 100) : currentTriggerPrice * (1 + currentSL / 100);
            //        this.LogInfo($"------ order TriggerPrice: {triggerPrice}  mainPrice:{currentTriggerPrice}  stoploss:{currentSL} orderExtID:{order?.ExtId} Direction:{order?.Direction}");
            //        //Order stoporder = new Order
            //        //{
            //        //    QuantityToFill = order!.QuantityToFill,
            //        //    Direction = order.Direction == OrderDirections.Buy ? OrderDirections.Sell : OrderDirections.Buy,
            //        //    Type = OrderTypes.Stop,
            //        //    Portfolio = order.Portfolio,
            //        //    Security = order.Security,
            //        //    ExpiryDate = order.ExpiryDate,
            //        //    OCOGroup = order.OCOGroup,
            //        //    TriggerPrice = triggerPrice
            //        //};
            //        //OpenOrderAsync(stoporder).Wait();
            //        return;
            //    }
            //    if (MainOrder != null && SLOrder == null)
            //    {
            //        SLOrder = order?.Clone();
            //        this.LogInfo($"------ Stoporder TriggerPrice: ");
            //        currentConsideringID = -1;
            //        return;
            //    }
            //    if (MainOrder != null && SLOrder != null)
            //    {
            //        MainOrder = null;
            //        SLOrder = null;
            //        currentBE = -1;
            //        currentSL = -1;
            //        isActivateOrder = false;
            //        currentTriggerPrice = -1;
            //        currentConsideringID = -1;
            //        return;
            //    }
            //    else
            //    {
            //        isActivateOrder = false;
            //    }
            //    //    //isActivateOrder[order.ExtId % 100000] = false;
            //}
            //else
            //{
            //    this.LogInfo($"Index:{order.ExtId} Expiry:{order.ExpiryDate}, Type:{order.Type} Quantity:{order.QuantityToFill}" +
            //        $" Direction:{order.Direction} OCO:{order.OCOGroup} TriggerPrice:{order.TriggerPrice} Price: {order.Price} ");
            //    isActivateOrder = false;
            //    currentConsideringID = -1;
            //}
        }


        protected override void OnPositionChanged(Position position)
        {
            //if (currentBE == -1) return;
            //if (MainOrder == null || SLOrder == null) return;
            ////if(position.)
            //if (MainOrder.Direction == OrderDirections.Buy)
            //{
            //    if (currentPrice >= position.AveragePrice * (1 + currentBE / 100))
            //    {
            //        Order newOrder = SLOrder.Clone();
            //        newOrder.TriggerPrice = position.AveragePrice;
            //        ModifyOrder(SLOrder, newOrder);
            //    }
            //}
            //else if (MainOrder.Direction == OrderDirections.Sell)
            //{
            //    if (currentPrice <= position.AveragePrice * (1 - currentBE / 100))
            //    {
            //        Order newOrder = SLOrder.Clone();
            //        newOrder.TriggerPrice = position.AveragePrice;
            //        ModifyOrder(SLOrder, newOrder);
            //    }
            //}
            //base/**/.OnPositionChanged(position);
        }

        public override bool ProcessKeyUp(KeyEventArgs e)
        {
            //if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            this.LogInfo("Key Insert is pressed.");
            if (e.Key == Key.Insert)
            {
                if (controlPanel != null)
                {
                    controlPanel.Show();
                }
            }
            return false;
        }
    }
}