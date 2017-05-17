using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukeNukemProject.Model
{
    public class Overview
    {
        public string OrderId { get; set; }
        public string CustName { get; set; }
        public string CustPhone { get; set; }
        public string ItemDetail { get; set; }
        public string ReadyAt { get; set; }
        public string PurchaseType { get; set; }
        public string PaymentType { get; set; }
        public string CustomerOther { get; set; }
        public string TotalPrice { get; set; }

        public Overview(
            string id, string name, string phone, string details, string readyAt,
            string purchaseType, string paymentType, string other, string totalPrice
            /*,double timer*/)
        {
            OrderId = id;
            CustName = name;
            CustPhone = phone;
            ItemDetail = details;
            ReadyAt = readyAt;
            PurchaseType = purchaseType;
            PaymentType = paymentType;
            CustomerOther = other;
            TotalPrice = totalPrice;
            //Timer = timer;
        }

        public Overview()
        {

        }

    }
}
