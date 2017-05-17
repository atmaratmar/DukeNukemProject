using DukeNukemProject.Model;
using DukeNukemProject.ViewModel;
using System.Windows.Controls;

namespace DukeNukemTests.TestResources
{
    public class FakeDatabase
    {
        public bool saveCustomer(object[] parameter)
        {
            int result = 0;
            bool isit = false;
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
            if (int.TryParse(phone, out result) && name.Length > 1)
            {
                isit = true;
                Customer c = new Customer(phone, name, address);
                Messenger.Instance.CustomerMessanger = c;
            }
            return isit;
        }
    }
}
