using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbenHedgeStrategy
{
    public static class GlobalVariables1
    {

        public static bool IsShowControlPanel;

        private static decimal _price1;

        public static decimal Price1
        {
            get { return _price1; }
            set {
                if (_price1 != value)
                {
                    _price1 = value;
                    //GlobalVariablesProxy.Instance.OnPropertyChanged(nameof(GlobalVariablesProxy.Price1));
                }
            }
        }
    
        public static decimal Price2;

        public static decimal Price3;
       
        public static decimal Price4;
        
        public static int Amount1;
         

        public static int Amount2;
         

        public static int Amount3;
        

        public static int Amount4;
         
        public static int Side1;
         
        public static int Side2;
        
        public static int Side3;
        
        public static int Side4;
        
        public static int Type1;
        

        public static int Type2;
         
        public static int Type3;
        
        public static int Type4;
       
        public static int Cross1;
      
        public static int Cross2;
        
        public static int Cross3;
        
        public static int Cross4;
        
        public static int Stoploss1;
      
        public static int Stoploss2;
       
        public static int Stoploss3;
        
        public static int Stoploss4;
      
        public static int Breakeven1;
       
        public static int Breakeven2;
        
        public static int Breakeven3;
        

        public static int Breakeven4;
        
        public static DateOnly CloseDate1;
         
        public static DateOnly CloseDate2;
      
        public static DateOnly CloseDate3;
       
        public static DateOnly CloseDate4;
        
        public static TimeOnly CloseTime1;
       
        public static TimeOnly CloseTime2;
        
        public static TimeOnly CloseTime3;
        
        public static TimeOnly CloseTime4;
        
        public static bool Enable1;
        
        public static bool Enable2;
        
        public static bool Enable3;
        
        public static bool Enable4;
       
        public static decimal Maxdrawdown;
    }
}
