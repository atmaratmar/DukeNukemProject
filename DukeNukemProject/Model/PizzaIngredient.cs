using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DukeNukemProject.Model
{
    public class PizzaIngredient : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public PizzaIngredient(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public PizzaIngredient(string name)
        {
            Name = name;
        }

        private bool ingredientIsChecked;

        public bool IngredientIsChecked
        {
            get { return ingredientIsChecked; }
            set
            {
                ingredientIsChecked = value;
                OnPropertyChanged("IngredientIsChecked");
            }
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
