using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using DukeNukemProject.Model;
using DukeNukemProject.ViewModel.Commands;
using System.Windows.Input;
using DukeNukemProject.Model.Database;
using System.Windows;
using DukeNukemProject.ViewModel.Resources;

namespace DukeNukemProject.ViewModel
{
    public class SelectionViewModel : INotifyPropertyChanged
    {
        private SingleParameterCommand addPizzaCommand;
        private SingleParameterCommand fillPizzaCommand;
        private SingleParameterCommand fillPortionCommand;
        StoredProcedures SP;
        public Pizza P { get; set; }
        public Portion Pp { get; set; }
        public List<Pizza> PizzaTempList { get; set; }
        public List<string> ItemDetails { get; set; }
        public string GetPizzaNumber { get; set; }
        public string GetPizzaName { get; set; }
        public TimeSpan GetPizzaTime { get; set; }
        public string GetPortionName { get; set; }
        public double GetPortionPrice { get; set; }
        public List<Portion> GetPizzaPortion { get; set; }
        public List<PizzaIngredient> GetIngredients { get; set; }
        public List<Pizza> GetPizzas { get; set; }
        private ObservableCollection<Pizza> pizzas;
        public ObservableCollection<Pizza> Pizzas
        {
            get { return pizzas; }
            set
            {
                pizzas = value;
                OnPropertyChanged("Pizzas");
            }
        }
        private string pizzaQuantity;
        public string PizzaQuantity
        {
            get { return pizzaQuantity; }
            set
            {
                pizzaQuantity = value;
                OnPropertyChanged("Quantity");
            }
        }
        private bool pizzaSelectionDropDown;
        public bool PizzaSelectionDropDown
        {
            get { return pizzaSelectionDropDown; }
            set
            {
                pizzaSelectionDropDown = value;
                OnPropertyChanged("PizzaSelectionDropDown");
            }
        }
        private ObservableCollection<Portion> portions;
        public ObservableCollection<Portion> Portions
        {
            get { return portions; }
            set
            {
                portions = value;
                OnPropertyChanged("Portions");
            }
        }
        private ObservableCollection<PizzaIngredient> ingredients;
        public ObservableCollection<PizzaIngredient> Ingredients
        {
            get { return ingredients; }
            set
            {
                ingredients = value;
                OnPropertyChanged("Ingredients");
            }
        }
        public SelectionViewModel()
        {
            ItemDetails = new List<string>();
            Pizzas = new ObservableCollection<Pizza>();
            Portions = new ObservableCollection<Portion>();
            Ingredients = new ObservableCollection<PizzaIngredient>();
            addPizzaCommand = new SingleParameterCommand(addPizza, canAddPizza);
            fillPizzaCommand = new SingleParameterCommand(fillPizza, canFillPizza);
            fillPortionCommand = new SingleParameterCommand(fillPortion, canFillPortion);
            SP = new StoredProcedures();
            GetPizzas = new List<Pizza>();
            loadPizzas();
        }

        //LOADS PIZZAS
        public void loadPizzas()
        {
            Pizzas.Add(new Pizza("- - SELECT PIZZA -"));
            bool resultIsNotEmpty = SP.MultiRowQuerySP("usp_GetAllPizzas");
            if (resultIsNotEmpty)
            {
                foreach (var row in SP.DicResult)
                {
                    List<string> temp = new List<string>();
                    temp = row.Value;
                    string number = temp[1];
                    string name = temp[3];
                    string makingTime = temp[4];
                    Pizzas.Add(new Pizza(number, name, SafeParser.Instance.StringToTimespan(makingTime)));
                }
            }
        }

        //LOADS INGREDIENTS
        public ICommand FillPortionOnClick
        {
            get { return fillPortionCommand; }
        }
        public void fillPortion(object obj)
        {
            string pizzaNumber = GetPizzaNumber;
            GetPortionName = obj as string;
            Dictionary<string, string> targetParams = new Dictionary<string, string>();
            targetParams.Add("@number", GetPizzaNumber);
            targetParams.Add("@name", GetPortionName);
            bool resultIsNotEmpty = SP.MultiRowQuerySP("usp_GetIngredientsByNumberAndPortion", targetParams);
            if (resultIsNotEmpty)
            {
                foreach (var row in SP.DicResult)
                {
                    List<string> temp = new List<string>();
                    temp = row.Value;
                    string name = temp[0];
                    bool theNameIsEqual = false;
                    foreach (var ingredient in Ingredients)
                    {
                        if (ingredient.Name == name)
                        {
                            theNameIsEqual = true;
                        }
                    }
                    if (!theNameIsEqual)
                    {
                        Ingredients.Add(new PizzaIngredient(name));
                    }
                }
            }
        }
        public bool canFillPortion()
        {
            return true;
        }

        //LOADS PORTIONS
        public ICommand FillPizzaOnClick
        {
            get { return fillPizzaCommand; }
        }
        public void fillPizza(object obj)
        {
            string fullName = obj as string;
            GetPizzaNumber = fullName.Substring(0, 3);
            GetPizzaName = fullName.Substring(6);
            Dictionary<string, string> targetParams = new Dictionary<string, string>();
            targetParams.Add("@number", GetPizzaNumber);
            bool resultIsNotEmpty = SP.MultiRowQuerySP("usp_GetPortionsByNumber", targetParams);
            if (resultIsNotEmpty)
            {
                foreach (var row in SP.DicResult)
                {
                    List<string> temp = new List<string>();
                    temp = row.Value;
                    string name = temp[0];
                    double price = SafeParser.Instance.StringToDouble(temp[1]);
                    bool theNameIsEqual = false;
                    foreach(var portion in Portions)
                    {
                        if(portion.Name == name)
                        {
                            theNameIsEqual = true;
                        }
                    }
                    if(!theNameIsEqual)
                    {
                        Portions.Add(new Portion(name, price));
                    }
                }
            }
        }
        public bool canFillPizza()
        {
            return true;
        }
        public ICommand addPizzaOnClick
        {
            get { return addPizzaCommand; }

        }

        //CREATES PIZZAS SELECTION AND ADD THEME TO STRING LIST
        public void addPizza(object obj)
        {
            Dictionary<string, string> targetParams = new Dictionary<string, string>();
            targetParams.Add("@number", GetPizzaNumber);
            bool resultIsNotEmpty = SP.MultiRowQuerySP("usp_GetTimeByNumber", targetParams);
            if (resultIsNotEmpty)
            {
                foreach (var row in SP.DicResult)
                {
                    List<string> temp = new List<string>();
                    temp = row.Value;
                    string timeString = temp[0];
                    GetPizzaTime = SafeParser.Instance.StringToTimespan(timeString); 
                }
            }
            GetIngredients = new List<PizzaIngredient>();
            foreach (var ingredient in Ingredients)
            {
                if (ingredient.IngredientIsChecked)
                {
                    GetIngredients.Add(ingredient);
                }
            }
            targetParams = new Dictionary<string, string>();
            targetParams.Add("@number", GetPizzaNumber);
            targetParams.Add("@name", GetPortionName);
            resultIsNotEmpty = SP.MultiRowQuerySP("usp_GetPriceByNumberAndPortion", targetParams);
            if (resultIsNotEmpty)
            {
                foreach (var row in SP.DicResult)
                {
                    List<string> temp = new List<string>();
                    temp = row.Value;
                    string portionPrice = temp[0];
                    GetPortionPrice = SafeParser.Instance.StringToDouble(portionPrice); 
                }
            }
            Pp = new Portion(GetPortionName, GetPortionPrice, GetIngredients);
            GetPizzaPortion = new List<Portion>();
            GetPizzaPortion.Add(Pp);
            P = new Pizza(GetPizzaNumber, GetPizzaName, GetPizzaTime, GetPizzaPortion);
            //CLEARS THE LIST WHEN IS SEARCHED A NEW CUSTOMER
            if (Messenger.Instance.IsANewCustomerMessenger)
            {
                GetPizzas.Clear();
                ItemDetails.Clear();
            }
            PizzaTempList = new List<Pizza>();
            for (int i = 0; i < SafeParser.Instance.StringToInt(PizzaQuantity); i++)
            {
                GetPizzas.Add(P);
                PizzaTempList.Add(P);
            }
            Messenger.Instance.PizzasMessenger = GetPizzas;
            assembleSelection();
            Messenger.Instance.IsANewCustomerMessenger = false;
        }
        public bool canAddPizza()
        {
            return true;
        }

        //CONVERTS PIZZAS INTO STRING LIST OF PIZZAS
        public void assembleSelection()
        {
            string result = "";
            List<string> clones = new List<string>();
            int count = 1;
            double? price = 1;
            foreach (var pizza in PizzaTempList)
            {
                foreach (var portion in pizza.Portions)
                {
                    price = portion.Price + (portion.Ingredients.Count * 10);
                    result = pizza.FullName + " " + portion.Name + " " + portion.Ingredients.Count.ToString() + " toppings " + price + " kr";
                }
                if (ItemDetails.Contains(result))
                {
                    count++;
                    clones.Add(result);
                }
                else
                {
                    ItemDetails.Add(result);
                }
            }
            if (clones.Count > 0)
            {
                ItemDetails.Add(clones[0] + " X" + count);
                ItemDetails.Remove(result);
            }
            Messenger.Instance.ItemDetailsMessenger = ItemDetails;
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
