using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DukeNukemProject.Model;
using System.ComponentModel;

namespace DukeNukemProject.ViewModel
{
    public class Messenger : INotifyPropertyChanged
    {
        private static Messenger instance;

        private Messenger() { }

        public static Messenger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Messenger();
                }
                return instance;
            }
        }
        public Customer CustomerMessanger { get; set; }
        public Selection SelectionMessenger { get; set; }
        public Order OrderMessenger { get; set; }
        public List<Pizza> PizzasMessenger { get; set; }
        public List<Pizza> PizzasTempMessenger { get; set; }
        public List<string> ItemDetailsMessenger { get; set; }
        public bool IsANewCustomerMessenger { get; set; }
        


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
