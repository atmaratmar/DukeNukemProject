using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukeNukemProject.Model
{
    public class OtherType
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public TimeSpan MakingTime { get; set; }
        public double Price { get; set; }
        public OtherType(int id, string number, string name, TimeSpan makingTime, double price)
        {
            Id = id;
            Number = number;
            Name = name;
            MakingTime = makingTime;
            Price = price;
        }
        public OtherType(string number, string name, TimeSpan makingTime, double price)
        {
            
            Number = number;
            Name = name;
            MakingTime = makingTime;
            Price = price;
        }
    }
}
