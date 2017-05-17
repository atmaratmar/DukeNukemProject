using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukeNukemProject.Model
{
    public class Order
    {
        public int Id { get; set; }
        public string PurchaseType { get; set; }
        public string PaymentType { get; set; }
        public DateTime ReadyAt { get; set; }
        public double TotalPrice { get; set; }

        public List<Selection> Selections { get; set; }
        public Order(int id, string purchaseType, DateTime readyAt, double totalPrice, string paymentType = null, List<Selection> s = null)
        {

            Id = id;
            PurchaseType = purchaseType;
            ReadyAt = readyAt;
            TotalPrice = totalPrice;
            PaymentType = paymentType;
            Selections = s;
     
        }
        public Order(string purchaseType, DateTime readyAt, double totalPrice, string paymentType = null, List<Selection> s = null)
        {

            
            PurchaseType = purchaseType;
            ReadyAt = readyAt;
            TotalPrice = totalPrice;
            PaymentType = paymentType;
            Selections = s;

        }
        public Order()
        {

        }
    }
}
