using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukeNukemProject.Model
{
    public class Portion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double ?Price { get; set; }

        public List<PizzaIngredient> Ingredients { get; set; }

        public Portion(string name, double? price = null, List<PizzaIngredient> ingredients = null)
        {
            Name = name;
            Price = price;
            Ingredients = ingredients;
        }
        public Portion(int id, string name, double? price = null, List<PizzaIngredient> ingredients = null)
        {
            Id = id;
            Name = name;
            Price = price;
            Ingredients = ingredients;
        }
        public Portion()
        {

        }
    }
}
