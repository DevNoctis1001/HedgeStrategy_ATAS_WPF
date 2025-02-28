using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RubbenHedgeStrategy
{ 
    public class ControlModels :INotifyPropertyChanged
    {

        // -------------- Textbox Price --------------------//

        private decimal _price1;
        private decimal _price2;
        private decimal _price3;
        private decimal _price4;

        // -------------- Textbox Amount --------------------//

        private decimal _amount1;
        private decimal _amount2;
        private decimal _amount3;
        private decimal _amount4;

        // -------------- Combobox Side --------------------//

        private string _selectedSideItem1;
        public List<string> SideItem1{get;set;}
        public string SelectedSideItem1
        {
            get { return _selectedSideItem1; }
            set {
                if (_selectedSideItem1 != value)
                {
                    _selectedSideItem1 = value;
                    OnPropertyChanged(nameof(SelectedSideItem1));
                }
            }
        }

        private string _selectedSideItem2;
        public List<string> SideItem2{get; set;}
        public string SelectedSideItem2
        {
            get { return _selectedSideItem2; }
            set
            {
                if (_selectedSideItem2 != value)
                {
                    _selectedSideItem2 = value;
                    OnPropertyChanged(nameof(SelectedSideItem2));
                }
            }
        }

        private string _selectedSideItem3;
        public List<string> SideItem3
        {
            get; set;
        }
        public string SelectedSideItem3
        {
            get { return _selectedSideItem3; }
            set
            {
                if (_selectedSideItem3 != value)
                {
                    _selectedSideItem3 = value;
                    OnPropertyChanged(nameof(SelectedSideItem3));
                }
            }
        }

        private string _selectedSideItem4;
        public List<string> SideItem4
        {
            get; set;
        }
        public string SelectedSideItem4
        {
            get { return _selectedSideItem4; }
            set
            {
                if (_selectedSideItem4 != value)
                {
                    _selectedSideItem4 = value;
                    OnPropertyChanged(nameof(SelectedSideItem4));
                }
            }
        }
        // -------------- Combobox Type --------------------//

        private string _selectedTypeItem1;
        public List<string> TypeItem1 { get; set; }
        public string SelectedTypeItem1
        {
            get { return _selectedTypeItem1; }
            set
            {
                if (_selectedTypeItem1 != value)
                {
                    _selectedTypeItem1 = value;
                    OnPropertyChanged(nameof(SelectedTypeItem1));
                }
            }
        } 

        private string _selectedTypeItem2;
        public List<string> TypeItem2 { get; set; }
        public string SelectedTypeItem2
        {
            get { return _selectedTypeItem2; }
            set
            {
                if (_selectedTypeItem2 != value)
                {
                    _selectedTypeItem2 = value;
                    OnPropertyChanged(nameof(SelectedTypeItem2));
                }
            }
        }

        private string _selectedTypeItem3;
        public List<string> TypeItem3 { get; set; }
        public string SelectedTypeItem3
        {
            get { return _selectedTypeItem3; }
            set
            {
                if (_selectedTypeItem3 != value)
                {
                    _selectedTypeItem3 = value;
                    OnPropertyChanged(nameof(SelectedTypeItem3));
                }
            }
        }

        private string _selectedTypeItem4;
        public List<string> TypeItem4 { get; set; }
        public string SelectedTypeItem4
        {
            get { return _selectedTypeItem4; }
            set
            {
                if (_selectedTypeItem4 != value)
                {
                    _selectedTypeItem4 = value;
                    OnPropertyChanged(nameof(SelectedTypeItem4));
                }
            }
        }
        // -------------- Combobox Cross --------------------//

        private string _selectedCrossItem1;
        public List<string> CrossItem1 { get; set; }
        public string SelectedCrossItem1
        {
            get { return _selectedCrossItem1; }
            set
            {
                if (_selectedCrossItem1 != value)
                {
                    _selectedCrossItem1 = value;
                    OnPropertyChanged(nameof(SelectedCrossItem1));
                }
            }
        }

        private string _selectedCrossItem2;
        public List<string> CrossItem2 { get; set; }
        public string SelectedCrossItem2
        {
            get { return _selectedCrossItem2; }
            set
            {
                if (_selectedCrossItem2 != value)
                {
                    _selectedCrossItem2 = value;
                    OnPropertyChanged(nameof(SelectedCrossItem2));
                }
            }
        }

        private string _selectedCrossItem3;
        public List<string> CrossItem3 { get; set; }
        public string SelectedCrossItem3
        {
            get { return _selectedCrossItem3; }
            set
            {
                if (_selectedCrossItem3 != value)
                {
                    _selectedCrossItem3 = value;
                    OnPropertyChanged(nameof(SelectedCrossItem3));
                }
            }
        }

        private string _selectedCrossItem4;
        public List<string> CrossItem4 { get; set; }
        public string SelectedCrossItem4
        {
            get { return _selectedCrossItem4; }
            set
            {
                if (_selectedCrossItem4 != value)
                {
                    _selectedCrossItem4 = value;
                    OnPropertyChanged(nameof(SelectedCrossItem4));
                }
            }
        }
        // -------------- Textbox Stoploss --------------------//

        private decimal _stoploss1;
        private decimal _stoploss2;
        private decimal _stoploss3;
        private decimal _stoploss4;

        // -------------- Textbox Breakeven--------------------//
        private decimal _breakeven1;
        private decimal _breakeven2;
        private decimal _breakeven3;
        private decimal _breakeven4;

        // -------------- Date & Time --------------------//
        private string _closeDate1;
        private string _closeTime1;

        private string _closeDate2;
        private string _closeTime2;

        private string _closeDate3;
        private string _closeTime3;

        private string _closeDate4;
        private string _closeTime4;

        // -------------- Checkbox Enable --------------------//
        private bool _enable1;
        private bool _enable2;
        private bool _enable3;
        private bool _enable4;

        // -------------- Textbox Maxdrawdown --------------------//
        private decimal _maxdrawdown =5;
        private decimal _currentPrice;
        private decimal _bestAskPrice;
        private decimal _bestBidPrice;
        private decimal _unPNL;
        private decimal _quantity;



        // -------------- Label StrategyStatus --------------------//
        private bool _isStrategyActive;

        public ControlModels()
        {
            SideItem1 = new List<string> { "Buy", "Sell" };
            SelectedSideItem1 = "Buy";
            SideItem2 = new List<string> { "Buy", "Sell" };
            SelectedSideItem2 = "Sell";
            SideItem3 = new List<string> { "Buy", "Sell" };
            SelectedSideItem3 = "Sell";
            SideItem4 = new List<string> { "Buy", "Sell" };
            SelectedSideItem4 = "Sell";

            //sideEntryItems1 = CollectionViewSource.GetDefaultView(_sideItems1);

            CrossItem1 = new List<string> { "Up", "Down" };
            SelectedCrossItem1 = "Up";
            CrossItem2 = new List<string> { "Up", "Down" };
            SelectedCrossItem2 = "Down";
            CrossItem3 = new List<string> { "Up", "Down" };
            SelectedCrossItem3 = "Down";
            CrossItem4 = new List<string> { "Up", "Down" };
            SelectedCrossItem4 = "Down";


            TypeItem1 = new List<string> { "Market", "Limit" };
            SelectedTypeItem1 = "Limit";
            TypeItem2 = new List<string> { "Market", "Limit" };
            SelectedTypeItem2 = "Market";
            TypeItem3 = new List<string> { "Market", "Limit" };
            SelectedTypeItem3 = "Market";
            TypeItem4 = new List<string> { "Market", "Limit" };
            SelectedTypeItem4 = "Limit";

            _closeDate1 = DateTime.Today.ToString("MM-dd-yyyy");
            _closeDate2 = DateTime.Today.ToString("MM-dd-yyyy");
            _closeDate3 = DateTime.Today.ToString("MM-dd-yyyy");
            _closeDate4 = DateTime.Today.ToString("MM-dd-yyyy");
            _closeTime1 = DateTime.Now.AddMinutes(20).TimeOfDay.ToString("hh\\:mm\\:ss");
            _closeTime2 = DateTime.Now.AddMinutes(20).TimeOfDay.ToString("hh\\:mm\\:ss");
            _closeTime3 = DateTime.Now.AddMinutes(20).TimeOfDay.ToString("hh\\:mm\\:ss");
            _closeTime4 = DateTime.Now.AddMinutes(20).TimeOfDay.ToString("hh\\:mm\\:ss");

            _amount1 = 1m;
            _price1 = 88000m;
            _stoploss1 = 0.3m;
            _breakeven1 =0.1m;

            _amount2 = 2m;
            _price2 = 87200m;
            _stoploss2 = 0.3m;
            _breakeven2 = 0.1m;

            _amount3 = 3m;
            _price3 = 95500m;
            _stoploss3 = 0.1m;
            _breakeven3 = 0.1m;

            _amount4 = 4m;
            _price4 = 95450;
            _stoploss4 = 0.1m;
            _breakeven4 = 0.1m;

            //_selectedSideItem1 = _sideItems1[1];

            _enable1 = true;
            _enable2 = true;
            _enable3 = true;
            _enable4 = true;

            _isStrategyActive= false;



        }
        public decimal Price1
        {
            get => _price1;
            set
            {
                if (_price1 != value)
                {
                    _price1 = value;
                    OnPropertyChanged(nameof(Price1));
                }
            }
        }

        public decimal Price2
        {
            get => _price2;
            set
            {
                if (_price2 != value)
                {
                    _price2 = value;
                    OnPropertyChanged(nameof(Price2));
                }
            }
        }

        public decimal Price3
        {
            get => _price3;
            set
            {
                if (_price3 != value)
                {
                    _price3 = value;
                    OnPropertyChanged(nameof(Price3));
                }
            }
        }

        public decimal Price4
        {
            get => _price4;
            set
            {
                if (_price4 != value)
                {
                    _price4 = value;
                    OnPropertyChanged(nameof(Price4));
                }
            }
        }

        public decimal Amount1
        {
            get => _amount1;
            set
            {
                if (_amount1 != value)
                {
                    _amount1 = value;
                    OnPropertyChanged(nameof(Amount1));
                }
            }
        }

        public decimal Amount2
        {
            get => _amount2;
            set
            {
                if (_amount2 != value)
                {
                    _amount2 = value;
                    OnPropertyChanged(nameof(Amount2));
                }
            }
        }

        public decimal Amount3
        {
            get => _amount3;
            set
            {
                if (_amount3 != value)
                {
                    _amount3 = value;
                    OnPropertyChanged(nameof(Amount3));
                }
            }
        }

        public decimal Amount4
        {
            get => _amount4;
            set
            {
                if (_amount4 != value)
                {
                    _amount4 = value;
                    OnPropertyChanged(nameof(Amount4));
                }
            }
        }
         

        // -----------------------  Stoploss ----------------------------------- //
        public decimal Stoploss1
        {
            get => _stoploss1;
            set
            {
                if (_stoploss1 != value)
                {
                    _stoploss1 = value;
                    OnPropertyChanged(nameof(Stoploss1));
                }
            }
        }

        public decimal Stoploss2
        {
            get => _stoploss2;
            set
            {
                if (_stoploss2 != value)
                {
                    _stoploss2 = value;
                    OnPropertyChanged(nameof(Stoploss2));
                }
            }
        }

        public decimal Stoploss3
        {
            get => _stoploss3;
            set
            {
                if (_stoploss3 != value)
                {
                    _stoploss3 = value;
                    OnPropertyChanged(nameof(Stoploss3));
                }
            }
        }

        public decimal Stoploss4
        {
            get => _stoploss4;
            set
            {
                if (_stoploss4 != value)
                {
                    _stoploss4 = value;
                    OnPropertyChanged(nameof(Stoploss4));
                }
            }
        }
        
        
        // -----------------------  Breakeven ----------------------------------- //
        public decimal BreakEven1
        {
            get => _breakeven1;
            set
            {
                if (_breakeven1 != value)
                {
                    _breakeven1 = value;
                    OnPropertyChanged(nameof(BreakEven1));
                }
            }
        }

        public decimal BreakEven2
        {
            get => _breakeven2;
            set
            {
                if (_breakeven2 != value)
                {
                    _breakeven2 = value;
                    OnPropertyChanged(nameof(BreakEven2));
                }
            }
        }

        public decimal BreakEven3
        {
            get => _breakeven3;
            set
            {
                if (_breakeven3 != value)
                {
                    _breakeven3 = value;
                    OnPropertyChanged(nameof(BreakEven3));
                }
            }
        }

        public decimal BreakEven4
        {
            get => _breakeven4;
            set
            {
                if (_breakeven4 != value)
                {
                    _breakeven4 = value;
                    OnPropertyChanged(nameof(BreakEven4));
                }
            }
        }

        // -----------------------  CloseDate ----------------------------------- //
        public string CloseDate1
        {
            get => _closeDate1;
            set
            {
                if (_closeDate1 != value)
                {
                    _closeDate1 = value;
                    OnPropertyChanged(nameof(CloseDate1));
                }
            }
        }
        public string CloseDate2
        {
            get => _closeDate2;
            set
            {
                if (_closeDate2 != value)
                {
                    _closeDate2 = value;
                    OnPropertyChanged(nameof(CloseDate2));
                }
            }
        }
        public string CloseDate3
        {
            get => _closeDate3;
            set
            {
                if (_closeDate3 != value)
                {
                    _closeDate3 = value;
                    OnPropertyChanged(nameof(CloseDate3));
                }
            }
        }
        public string CloseDate4
        {
            get => _closeDate4;
            set
            {
                if (_closeDate4 != value)
                {
                    _closeDate4 = value;
                    OnPropertyChanged(nameof(CloseDate4));
                }
            }
        }

        public string CloseTime1
        {
            get => _closeTime1;
            set
            {
                if (_closeTime1 != value)
                {
                    _closeTime1 = value;
                    OnPropertyChanged(nameof(CloseTime1));
                }
            }
        }
        public string CloseTime2
        {
            get => _closeTime2;
            set
            {
                if (_closeTime2 != value)
                {
                    _closeTime2 = value;
                    OnPropertyChanged(nameof(CloseTime2));
                }
            }
        }
        public string CloseTime3
        {
            get => _closeTime3;
            set
            {
                if (_closeTime3 != value)
                {
                    _closeTime3 = value;
                    OnPropertyChanged(nameof(CloseTime3));
                }
            }
        }
        public string CloseTime4
        {
            get => _closeTime4;
            set
            {
                if (_closeTime4 != value)
                {
                    _closeTime4 = value;
                    OnPropertyChanged(nameof(CloseTime4));
                }
            }
        }


        // ------------------------- Enable ------------------------------//
        public bool Enable1
        {
            get => _enable1;
            set
            {
                if (_enable1 != value)
                {
                    _enable1 = value;
                    OnPropertyChanged(nameof(Enable1));
                }
            }
        }

        public bool Enable2
        {
            get => _enable2;
            set
            {
                if (_enable2 != value)
                {
                    _enable2 = value;
                    OnPropertyChanged(nameof(Enable2));
                }
            }
        }

        public bool Enable3
        {
            get => _enable3;
            set
            {
                if (_enable3 != value)
                {
                    _enable3 = value;
                    OnPropertyChanged(nameof(Enable3));
                }
            }
        }

        public bool Enable4
        {
            get => _enable4;
            set
            {
                if (_enable4 != value)
                {
                    _enable4 = value;
                    OnPropertyChanged(nameof(Enable4));
                }
            }
        }

        // ------------------------- MaxDrawdown ------------------------------//
        public decimal Maxdrawdown
        {
            get => _maxdrawdown;
            set
            {
                if (_maxdrawdown != value)
                {
                    _maxdrawdown = value;
                    OnPropertyChanged(nameof(Maxdrawdown));
                }
            }
        }

        // -------------- Label StrategyStatus --------------------//
        public bool IsStrategyActive
        {
            get => _isStrategyActive;
            set
            {
                if (_isStrategyActive != value)
                {
                    _isStrategyActive = value;
                    OnPropertyChanged(nameof(IsStrategyActive));
                }
            }
        }

        public decimal CurrentPrice
        {
            get => _currentPrice;
            set
            {
                if(_currentPrice != value)
                {
                    _currentPrice = value;
                    OnPropertyChanged(nameof(CurrentPrice));
                }
            }
        }

        public decimal BestAskPrice
        {
            get => _bestAskPrice;
            set
            {
                if (value != _bestAskPrice)
                {
                    _bestAskPrice = value;
                    OnPropertyChanged(nameof(BestAskPrice));
                }
            }
        }

        public decimal BestBidPrice
        {
            get => _bestBidPrice;
            set
            {
                if (value != _bestBidPrice)
                {
                    _bestBidPrice = value;
                    OnPropertyChanged(nameof(BestBidPrice));
                }
            }
        }
        public decimal UnPNL
        {
            get => _unPNL;
            set
            {
                if (value != _unPNL)
                {
                    _unPNL = value;
                    OnPropertyChanged(nameof(UnPNL));
                }
            }
        }

        public decimal Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
