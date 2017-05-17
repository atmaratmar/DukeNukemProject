using System;
using System.Collections.Generic;
using DukeNukemProject.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using DukeNukemProject.ViewModel.Commands;
using System.Windows.Input;
using DukeNukemProject.ViewModel.Resources;
using DukeNukemProject.Model.Database;
using System.Globalization;

namespace DukeNukemProject.ViewModel
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        private SingleParameterCommand addOrderCommand;
        private SingleParameterCommand refreshOrderCommand;
        Selection sel;
        Order order;
        StoredProcedures sp;
        public Customer Cust { get; set; }
        public List<Pizza> Pizzas { get; set; }
        public List<string> ItemDetails { get; set; }
        public List<Selection> Selections { get; set; }
        private string purchaseType;
        public string PurchaseType
        {
            get
            {
                if (DeliveryIsChecked)
                {
                    purchaseType = "Delivery";
                }
                else
                {
                    purchaseType = "Non Delivery";
                }
                return purchaseType;
            }
            set
            {
                purchaseType = value;
                OnPropertyChanged("DeliveryIsChecked");
                OnPropertyChanged("PurchaseType");
            }
        }
        private bool deliveryIsChecked;
        public bool DeliveryIsChecked
        {
            get { return deliveryIsChecked; }
            set
            {
                deliveryIsChecked = value;
                OnPropertyChanged("DeliveryIsChecked");
            }
        }
        private string paymentType;
        public string PaymentType
        {
            get
            {
                if (CashIsChecked)
                {
                    paymentType = "Cash";
                }
                else
                {
                    paymentType = "Noncash";
                }
                return paymentType;
            }
            set
            {
                purchaseType = value;
                OnPropertyChanged("CashIsChecked");
                OnPropertyChanged("PaymentType");
            }
        }
        private bool cashIsChecked;
        public bool CashIsChecked
        {
            get { return cashIsChecked; }
            set
            {
                cashIsChecked = value;
                OnPropertyChanged("CashIsChecked");
            }
        }
        private ObservableCollection<string> obsItemDetails;
        public ObservableCollection<string> ObsItemDetails
        {
            get { return obsItemDetails; }
            set
            {
                obsItemDetails = value;
                OnPropertyChanged("ObsItemDetails");
            }
        }
        private string totalPrice;
        public string TotalPrice
        {
            get { return totalPrice; }
            set
            {
                totalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }
        private string custName;
        public string CustName
        {
            get { return custName; }
            set
            {
                custName = value;
                OnPropertyChanged("CustName");
            }
        }
        private string custPhone;
        public string CustPhone
        {
            get { return custPhone; }
            set
            {
                custPhone = value;
                OnPropertyChanged("CustPhone");
            }
        }
        private string custAddress;
        public string CustAddress
        {
            get { return custAddress; }
            set
            {
                custAddress = value;
                OnPropertyChanged("CustAddress");
            }
        }
        public OrderViewModel()
        {
            addOrderCommand = new SingleParameterCommand(addOrder, canAddOrder);
            refreshOrderCommand = new SingleParameterCommand(refreshOrder, canRefreshOrder);
            Pizzas = new List<Pizza>();
            ItemDetails = new List<string>();
            ObsItemDetails = new ObservableCollection<string>();
            Selections = new List<Selection>();
            sp = new StoredProcedures();
        }

        //DISPLAY DATA ABOUT THE CUSTOMER AND ITEM DETAILS 
        public ICommand RefreshOrderOnClick
        {
            get { return refreshOrderCommand; }
        }
        public void refreshOrder(object obj)
        {
            Cust = Messenger.Instance.CustomerMessanger;
            ItemDetails = Messenger.Instance.ItemDetailsMessenger;
            if (ItemDetails!=null && Cust != null)
            {
                CustName = Cust.Name;
                CustPhone = Cust.Phone;
                CustAddress = Cust.Address;
                TotalPrice = getPizzaPrice().ToString();
                ObsItemDetails = new ObservableCollection<string>(ItemDetails);
            }
            else
            {
                MessageBox.Show("Plese, insert customer and select items");
            }
        }
        public bool canRefreshOrder()
        {
            return true;
        }
        public ICommand AddOrderOnClick
        {
            get { return addOrderCommand; }
        }
        
        //SAVES ORDER AND CONNECTS ORDER WITH SELECTION AND CUSTOMER
        public void addOrder(object obj)
        {
            if (ItemDetails.Count > 0 && Cust != null)
            {
                //SAVES ORDER
                string stringTimeNow = String.Format(CultureInfo.InvariantCulture, "{0:dd\\/MM\\/yyyy HH:mm:ss}", DateTime.Now);
                DateTime selDateTime = SafeParser.Instance.StringToDateTime(stringTimeNow);
                sel = new Selection(Cust, selDateTime, ItemDetails);
                Selections = new List<Selection>();
                Selections.Add(sel);
                Dictionary<string, string> targetParams = new Dictionary<string, string>();
                targetParams.Add("@purchaseType", PurchaseType);
                targetParams.Add("@paymentType", PaymentType);
                string readyAt = string.Format(CultureInfo.InvariantCulture, "{0:dd\\/MM\\/yyyy HH:mm:ss}", getPizzaTime());
                targetParams.Add("@readyAt", readyAt);
                targetParams.Add("@totalPrice", getPizzaPrice().ToString());
                sp.QuerySP("usp_SaveOrder", targetParams);
                string stringOrderId = sp.Result[0];
                int orderId = SafeParser.Instance.StringToInt(stringOrderId);
                order = new Order(orderId, PurchaseType, getPizzaTime(), getPizzaPrice(), PaymentType, Selections);
                //SAVES SELECTION
                targetParams = new Dictionary<string, string>();
                string stringSelectionDate = string.Format(CultureInfo.InvariantCulture, "{0:dd\\/MM\\/yyyy HH:mm:ss}", sel.Date);
                targetParams.Add("@selectionDate", stringSelectionDate);
                string stringMenuId = (1).ToString();
                targetParams.Add("@menuId", stringMenuId);
                targetParams.Add("@customerId", Cust.Id.ToString());
                targetParams.Add("@orderId", stringOrderId);
                sp.QuerySP("usp_SaveSelection", targetParams);
                string stringSelectionId = sp.Result[0];
                int selectionId = SafeParser.Instance.StringToInt(stringSelectionId);

                foreach(var item in ItemDetails)
                {
                    //SAVES ITEMS
                    targetParams = new Dictionary<string, string>();
                    targetParams.Add("@details",item);
                    sp.QuerySP("usp_SaveItems", targetParams);
                    string stringItemId = sp.Result[0];
                    int itemId = SafeParser.Instance.StringToInt(stringItemId);
   
                    //SAVES ITEMSELECTIONS
                    targetParams = new Dictionary<string, string>();
                    targetParams.Add("@selectionId", stringSelectionId);
                    targetParams.Add("@itemId", stringItemId);
                    sp.NonQuerySP("usp_SaveItemSelection", targetParams);
                }
            }
            else
            {
                MessageBox.Show("Plese, insert customer and select items");
            }
        }
        public bool canAddOrder()
        {
            return true;
        }

        //GETS TOTAL TIME PER PIZZAS SELECTED
        public DateTime getPizzaTime()
        {
            TimeSpan totalTimePerPizza = new TimeSpan(0, 0, 0);
            DateTime date = DateTime.Now;
            DateTime maxReadyAt = getMaxReadyAt();
            Pizzas = Messenger.Instance.PizzasMessenger;
            int pizzasCounter = 0;
            int totalPizzasNumber = 0;
            int multiplier = 0;
            foreach (var pizza in Pizzas)
            {
                pizzasCounter++;
                totalPizzasNumber++;
                if(pizzasCounter == 6)
                {
                    totalTimePerPizza += pizza.MakingTime.Value;
                    pizzasCounter = 0;
                    multiplier++;
                }
            }
            if(totalPizzasNumber < 6)
            {
                totalTimePerPizza = new TimeSpan(0, 15, 0);
            }
            else
            {
                totalPizzasNumber = totalPizzasNumber - (multiplier * 6);
                totalTimePerPizza += new TimeSpan(0, totalPizzasNumber, 0);
            }
            DateTime result = maxReadyAt.Add(totalTimePerPizza);
            return result;
        }

        //GET TOTAL PRICE PER PIZZAS SELECTED
        public double getPizzaPrice()
        {
            Pizzas = Messenger.Instance.PizzasMessenger;
            double totalPrice = 0;
            foreach(var pizza in Pizzas)
            {
                foreach(var portion in pizza.Portions)
                {
                    totalPrice += portion.Price.Value + (10 * portion.Ingredients.Count);
                }
            }
            return totalPrice;
        }

        //GETS THE LAST DATE TO QUEUE THE ORDERS
        public DateTime getMaxReadyAt()
        {
            Dictionary<string, string> targetParams = new Dictionary<string, string>();
            DateTime maxReadyAt;
            if(sp.QuerySP("usp_GetMaxDateFromId", targetParams))
            {
                string stringMaxReadyAt = sp.Result[0];
                string stringTimeNow = String.Format(CultureInfo.InvariantCulture, "{0:dd\\/MM\\/yyyy HH:mm:ss}", DateTime.Now);
                if(SafeParser.Instance.StringToDateTime(stringTimeNow) > SafeParser.Instance.StringToDateTime(stringMaxReadyAt))
                {
                    maxReadyAt = SafeParser.Instance.StringToDateTime(stringTimeNow);
                }
                else
                {
                    maxReadyAt = SafeParser.Instance.StringToDateTime(stringMaxReadyAt);
                }  
            }
            else
            {
                string stringTimeNow = String.Format(CultureInfo.InvariantCulture, "{0:dd\\/MM\\/yyyy HH:mm:ss}", DateTime.Now);
                maxReadyAt = SafeParser.Instance.StringToDateTime(stringTimeNow);
            }
            return maxReadyAt;
        }

        //STANDARD ON PROPERTY CHANGED PATTERN
        #region PROPERTY CHANGE
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        #endregion

    }
}
