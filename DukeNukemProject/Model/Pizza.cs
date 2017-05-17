using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace DukeNukemProject.Model
{
    public class Pizza : INotifyPropertyChanged
    {
        private string number;

        public string Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
               
            }
        }
        private TimeSpan? makingTime;

        public TimeSpan? MakingTime
        {
            get { return makingTime; }
            set
            {
                makingTime = value;
                OnPropertyChanged("MakingTime");
            }
        }
        public List<Portion> Portions { get; set; }

        private string fullName;

        public string FullName
        {
            get { return Number + " - " + Name; }
            set
            {
                fullName = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("Number");
                OnPropertyChanged("FullName");
            }
        }
        public Pizza(string num, string name = null, TimeSpan? makingTime = null, List<Portion> portions = null)
        {
            Number = num;
            Name = name;
            MakingTime = makingTime;
            Portions = portions;

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
