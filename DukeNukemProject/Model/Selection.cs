using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace DukeNukemProject.Model 
{
    public class Selection : INotifyPropertyChanged
    {
        public Customer Customer { get; set; }
        public DateTime Date { get; set; }
        public List<string> ItemDetails { get; set; }
        public Selection(Customer cust, DateTime date, List<string> itemDetails)
        {
            Customer = cust;
            Date = date;
            ItemDetails = itemDetails;
        }

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
