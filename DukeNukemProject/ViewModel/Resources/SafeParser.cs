using System;
using System.Globalization;

namespace DukeNukemProject.ViewModel.Resources
{
    public class SafeParser
    {
        private static SafeParser instance;

        private SafeParser() { }

        public static SafeParser Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SafeParser();
                }
                return instance;
            }
        }
        public int StringToInt(string input)
        {
            int output;
            Int32.TryParse(input, out output);
            return output;
            
        }

        public double StringToDouble(string input)
        {
            double output;
            Double.TryParse(input, out output);
            return output;
        }

        public TimeSpan StringToTimespan(string input)
        {
            TimeSpan output;
            TimeSpan.TryParse(input, out output);
            return output;
        }

        public DateTime StringToDateTime(string input)
        {
            DateTime output;
            DateTime.TryParseExact(input, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out output);
            return output;
        }

       
    }
}
