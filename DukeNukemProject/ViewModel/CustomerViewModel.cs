using System.Collections.Generic;
using DukeNukemProject.ViewModel.Commands;
using DukeNukemProject.Model;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Controls;
using DukeNukemProject.Model.Database;
using System.Windows;
using DukeNukemProject.ViewModel.Resources;

namespace DukeNukemProject.ViewModel
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private ParameterCommand saveCustomerCommand;
        private SingleParameterCommand getCustomerByPhoneCommand;
        StoredProcedures sp = new StoredProcedures();
        public Customer Cust { get; set; }
        public bool getAnyCustomer { get; set; }
        private string custPhone;
        public string CustPhone
        {
            get
            {
                return custPhone;

            }
            set
            {
                custPhone = value;
                OnPropertyChanged("CustPhone");
            }
        }
        private string custName;
        public string CustName
        {
            get
            {
                return custName;
            }
            set
            {
                custName = value;
                OnPropertyChanged("CustName");
            }
        }
        private string custAddress;
        public string CustAddress
        {
            get
            {
                return custAddress;
            }
            set
            {
                custAddress = value;
                OnPropertyChanged("CustAddress");
            }
        }
        private string custEmail;
        public string CustEmail
        {
            get
            {
                return custEmail;
            }
            set
            {
                custEmail = value;
                OnPropertyChanged("CustEmail");
            }
        }
        private string custOther;
        public string CustOther
        {
            get
            {
                return custOther;
            }
            set
            {
                custOther = value;
                OnPropertyChanged("CustOther");
            }
        }
        public CustomerViewModel()
        {
            saveCustomerCommand = new ParameterCommand(saveCustomer, canSaveCustomer);
            getCustomerByPhoneCommand = new SingleParameterCommand(getCustomerByPhone, canGetCustomerByPhone);
        }
        
        //SEARCHES CUSTOMER
        public ICommand GetCustomerByPhone
        {
            get { return getCustomerByPhoneCommand; }

        }
        public void getCustomerByPhone(object phone)
        {
            Dictionary<string, string> targetParams = new Dictionary<string, string>();
            bool phoneHasRightFormat = Validator.Instance.IsPhoneNumber(phone.ToString());
            if (phoneHasRightFormat)
            {
                targetParams.Add("@phone", phone.ToString());
                bool lookUpPhoneNumber = sp.QuerySP("usp_GetCustomerByPhone", targetParams);
                if (lookUpPhoneNumber)
                {
                    int custId = SafeParser.Instance.StringToInt(sp.Result[0]);
                    CustName = sp.Result[1];
                    CustAddress = sp.Result[2];
                    CustEmail = sp.Result[3];
                    CustOther = sp.Result[4];
                    Cust = new Customer(custId, phone.ToString(), CustName, CustAddress, CustEmail, CustOther);
                    Messenger.Instance.CustomerMessanger = Cust;
                }
                else
                {
                    CustName = "";
                    CustAddress = "";
                    CustEmail = "";
                    CustOther = "";
                }
                getAnyCustomer = sp.QuerySP("usp_GetCustomerByPhone", targetParams);
                Messenger.Instance.IsANewCustomerMessenger = true;
            }
            else
            {
                MessageBox.Show("Insert the right phone format");
                CustPhone = "";
                CustName = "";
                CustAddress = "";
                CustEmail = "";
                CustOther = "";
            }
        }
        public bool canGetCustomerByPhone()
        {
            return true;
        }

        //SAVES CUSTOMER
        public ICommand SaveCustomerOnClick
        {
            get { return saveCustomerCommand; }

        }

        public void saveCustomer(object[] parameter)
        {
            Dictionary<string, string> targetParams = new Dictionary<string, string>();
            TextBox phoneTextBox = parameter[0] as TextBox;
            string phone = phoneTextBox.Text;
            TextBox nameTextBox = parameter[1] as TextBox;
            string name = nameTextBox.Text;
            TextBox addressTextBox = parameter[2] as TextBox;
            string address = addressTextBox.Text;
            TextBox emailTextBox = parameter[3] as TextBox;
            string email = emailTextBox.Text;
            TextBox otherTextBox = parameter[4] as TextBox;
            string other = otherTextBox.Text;
            getCustomerByPhone(phone);
            if(!getAnyCustomer && (phone != "") && (name != ""))
            {
                targetParams.Add("@name", name);
                targetParams.Add("@phone", phone);
                targetParams.Add("@address", address);
                targetParams.Add("@email", email);
                targetParams.Add("@other", other);
                sp.QuerySP("usp_SaveCustomer", targetParams);
                int custId = SafeParser.Instance.StringToInt(sp.Result[0]);
                Cust = new Customer(custId, phone, name, address, email, other);
                Messenger.Instance.CustomerMessanger = Cust;
            }
            else
            {
                if (getAnyCustomer) MessageBox.Show("Customer already exists ");
            }
        }

        public bool canSaveCustomer()
        {
            return true;
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

