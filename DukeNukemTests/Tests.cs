using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DukeNukemProject.ViewModel;
using DukeNukemProject.Model;
using DukeNukemProject.Model.Database;
using System.Windows.Controls;
using DukeNukemTests.TestResources;

namespace DukeNukemTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void CustomerViewModelGetCustomerByPhoneNumber()
        {
            // Look up customer by phone number.
            CustomerViewModel cvm = new CustomerViewModel();
            cvm.getCustomerByPhone("00010000");
            Assert.AreEqual(cvm.Cust.Phone, "00010000");
            Assert.AreEqual(cvm.CustName, "Jack Black");
            Assert.AreEqual(Messenger.Instance.CustomerMessanger.Id, cvm.Cust.Id);
            Assert.AreEqual(Messenger.Instance.CustomerMessanger, cvm.Cust);
        }
        [TestMethod]
        public void CustomerViewModelCreateCustomer()
        {
            //Create Customer with all values
            CustomerViewModel cvm = new CustomerViewModel();
            StoredProcedures sp = new StoredProcedures();

            TextBox Phone = new TextBox();
            Phone.Text = "00000000";
            TextBox Name = new TextBox();
            Name.Text = "Jack Black";
            TextBox Address = new TextBox();
            Address.Text = "Brogade 7";
            TextBox Email = new TextBox();
            Email.Text = "EmailAddress@dk.is";
            TextBox Other = new TextBox();
            Other.Text = "TestCase";

            object[] Parameters = { Phone, Name, Address, Email, Other };
            cvm.saveCustomer(Parameters);
            //Check if the customer in the messenger is the same the one we created.
            Assert.AreEqual(cvm.Cust, Messenger.Instance.CustomerMessanger);
            cvm.Cust = null;
            cvm.getCustomerByPhone(Phone.Text.ToString());
            //Look up the customer we created.
            Assert.AreEqual(Messenger.Instance.CustomerMessanger, cvm.Cust);

        }
        [TestMethod]
        public void CustomerViewModelCreateCustomerOnlyNameAndPhoneNumber()
        {
            //Create Customer with only name and phone number.
            CustomerViewModel cvm = new CustomerViewModel();
            StoredProcedures sp = new StoredProcedures();

            TextBox Phone = new TextBox();
            Phone.Text = "77777877";
            TextBox Name = new TextBox();
            Name.Text = "Jimmy Jump";
            TextBox Address = new TextBox();
            Address.Text = "";
            TextBox Email = new TextBox();
            Email.Text = "";
            TextBox Other = new TextBox();
            Other.Text = "";
            object[] Parameters = { Phone, Name, Address, Email, Other };
            cvm.saveCustomer(Parameters);
            //Check if the customer in the messenger is the same the one we created.
            Assert.AreEqual(cvm.Cust, Messenger.Instance.CustomerMessanger);
            cvm.Cust = null;
            cvm.getCustomerByPhone(Phone.Text.ToString());
            //Look up the customer we created.
            Assert.AreEqual(Messenger.Instance.CustomerMessanger, cvm.Cust);

        }
        [TestMethod]
        public void CustomerViewModelCreateCustomerFakeDB()
        {
            FakeDatabase fdb = new FakeDatabase();
            TextBox Phone = new TextBox();
            Phone.Text = "2222";
            TextBox Name = new TextBox();
            Name.Text = "Jimmy Jump";
            TextBox Address = new TextBox();
            Address.Text = "";
            TextBox Email = new TextBox();
            Email.Text = "";
            TextBox Other = new TextBox();
            Other.Text = "";
            object[] Parameters = { Phone, Name, Address, Email, Other };
            Assert.IsTrue(fdb.saveCustomer(Parameters));
            Assert.IsNotNull(Messenger.Instance.CustomerMessanger);
        }
        [TestMethod]
        public void SelectionViewModelLoadPizzas()
        {
            //Check if we can load all pizzas, 
            SelectionViewModel svm = new SelectionViewModel();
            StoredProcedures sp = new StoredProcedures();

            Assert.IsTrue(sp.MultiRowQuerySP("usp_GetAllPizzas"));
            svm.loadPizzas();
            //Check if the Pizza list is not empty
            Assert.IsNotNull(svm.Pizzas);
            //Check if the first name is Garlic bread
            Assert.AreEqual(svm.Pizzas[1].Name, "Garlic Bread");
            //Check if we have 8 items in the Pizzas list.
            Assert.AreEqual(8, svm.Pizzas.Count);

        }

        [TestMethod]
        public void SelectionViewModelGetDifferentPortions()
        {
            //Check if we can load all the portions for pizzas, 3 in total.
            SelectionViewModel svm = new SelectionViewModel();
            Assert.AreEqual(0, svm.Portions.Count);
            svm.fillPizza("200 - Garlic Bread");
            Assert.AreEqual(3, svm.Portions.Count);
        }
        [TestMethod]
        public void SelectionViewModelGetIngredents()
        {
            //Check if we can get all the ingredients.
            SelectionViewModel svm = new SelectionViewModel();
            Assert.AreEqual(0, svm.Ingredients.Count);
            svm.GetPizzaNumber = "200";
            svm.fillPortion("Deep");
            //Check if there are 7 ingredients in the Ingredients OC.
            Assert.AreEqual(7, svm.Ingredients.Count);
        }
        [TestMethod]
        public void SelectionViewModelCreatePizza()
        {
            //Check if we can create a pizza item to the order.
            SelectionViewModel svm = new SelectionViewModel();
            svm.GetPizzaNumber = "200";
            svm.GetPortionName = "Deep";
            svm.PizzaQuantity = "3";
            svm.addPizza(true);
            //Check how many pizzas are in the messenger.
            Assert.AreEqual(3, Messenger.Instance.PizzasMessenger.Count);
        }
        [TestMethod]
        public void SelectionViewModelCreatePizzaString()
        {
            //Check if we can create a order item and create a string from that. We are checking assembleSelection method.
            SelectionViewModel svm = new SelectionViewModel();
            svm.GetPizzaNumber = "200";
            svm.GetPortionName = "Deep";
            svm.PizzaQuantity = "1";
            svm.addPizza(true);
            //Check if the string looks like it should
            Assert.AreEqual("200 -  Deep 0 toppings 78 kr", Messenger.Instance.ItemDetailsMessenger[0]);
        }
        [TestMethod]
        public void OrderViewModelRefreshOrder()
        {
            //Check if order is in the OrderViewModel
            Customer c = new Customer(99, "25252525", "Joe Stone", "Kogeneiboegene 3");
            SelectionViewModel svm = new SelectionViewModel();
            svm.GetPizzaNumber = "200";
            svm.GetPortionName = "Deep";
            svm.PizzaQuantity = "1";
            svm.addPizza(true);
            Messenger.Instance.CustomerMessanger = c;
            OrderViewModel owm = new OrderViewModel();
            owm.refreshOrder(true);
            //Check if the customer is the same as the messenger
            Assert.AreEqual(c, owm.Cust);
            //Check if the name is the same as the messenger
            Assert.AreEqual(c.Name, owm.CustName);
            //Check if the ordered items are the same the messenger.
            Assert.AreEqual(Messenger.Instance.PizzasMessenger, owm.Pizzas);
        }
        [TestMethod]
        public void OrderViewModelCalculatePrice()
        {
            //Check if we get the correct price.
            SelectionViewModel svm = new SelectionViewModel();
            svm.GetPizzaNumber = "200";
            svm.GetPortionName = "Deep";
            svm.PizzaQuantity = "3";
            svm.addPizza(true);
            OrderViewModel owm = new OrderViewModel();
            Assert.AreEqual(234, owm.getPizzaPrice());
        }
        [TestMethod]
        public void OrderViewModelCalculatePriceWithExtraIngrdeients()
        {
            //Check if we get the correct price with extra ingdrenitens.
            SelectionViewModel svm = new SelectionViewModel();
            svm.GetPizzaNumber = "200";
            svm.GetPortionName = "Deep";
            svm.PizzaQuantity = "3";
            svm.Ingredients.Add(new PizzaIngredient("bacon"));
            svm.Ingredients[0].IngredientIsChecked = true;
            svm.addPizza(true);
            OrderViewModel owm = new OrderViewModel();
            Assert.AreEqual(264, owm.getPizzaPrice());
        }
        [TestMethod]
        public void OrderViewModelCreateOrder()
        {
            OverviewViewModel ovvm = new OverviewViewModel();
            ovvm.showOverview(true);
            //Counts how many orders are before.
            int ordercount = ovvm.Overviews.Count;
            OrderViewModel owm = new OrderViewModel();
            SelectionViewModel svm = new SelectionViewModel();
            CustomerViewModel cvm = new CustomerViewModel();
            //Find a customer that is in the database
            cvm.getCustomerByPhone("00090000");
            //Adds a pizza to his order
            svm.GetPizzaNumber = "200";
            svm.GetPortionName = "Deep";
            svm.PizzaQuantity = "1";
            svm.Ingredients.Add(new PizzaIngredient("bacon"));
            svm.Ingredients[0].IngredientIsChecked = true;
            svm.addPizza(true);
            //Refresh the order
            owm.refreshOrder(true);
            //Says its a delievery and that is not cash
            owm.DeliveryIsChecked = true;
            owm.CashIsChecked = false;
            //Adds the order to the Order database
            owm.addOrder(true);
            //Refreshes the order overview
            ovvm.showOverview(true);
            //Checks how many orders were before agains how many are now.
            Assert.AreNotEqual(ordercount, ovvm.Overviews.Count);
        }
        [TestMethod]
        public void OrderViewModelGetDateTime()
        {
            OrderViewModel owm = new OrderViewModel();
            DateTime dt = DateTime.Now;
            dt = dt.AddMinutes(15);
            SelectionViewModel svm = new SelectionViewModel();
            svm.GetPizzaNumber = "200";
            svm.GetPortionName = "Deep";
            svm.PizzaQuantity = "1";
            svm.Ingredients.Add(new PizzaIngredient("bacon"));
            svm.Ingredients[0].IngredientIsChecked = true;
            svm.addPizza(true);
            //Assert.AreEqual(dt, owm.getPizzaTime());
        }
        [TestMethod]
        public void OverviewViewModelGetData()
        {
            OverviewViewModel ovvm = new OverviewViewModel();
            Assert.AreEqual(0, ovvm.Overviews.Count);
            ovvm.showOverview(true);
            Assert.IsTrue(ovvm.Overviews.Count > 1);
        }
        [TestMethod]
        public void OverviewViewModelDeleteData()
        {
            OverviewViewModel ovvm = new OverviewViewModel();
            ovvm.showOverview(true);
            int ordercount = ovvm.Overviews.Count;
            ovvm.deleteRow(ovvm.Overviews[0] as Overview);
            ovvm.showOverview(true);
            Assert.AreNotEqual(ordercount, ovvm.Overviews.Count);
        }
    }
}
