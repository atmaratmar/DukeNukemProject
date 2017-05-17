using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukeNukemProject.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Other { get; set; }
        public Customer(string phone, string name, string address = null, string email = null, string other = null)
        {
            Name = name;
            Address = address;
            Phone = phone;
            Email = email;
            Other = other;
        }

        public Customer(int id, string phone, string name, string address, string email = null, string other = null)
        {
            Id = id;
            Name = name;
            Address = address;
            Phone = phone;
            Email = email;
            Other = other;
        }
        public Customer()
        {

        }
        


    }
}
