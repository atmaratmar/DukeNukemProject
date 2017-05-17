using System.Text.RegularExpressions;

namespace DukeNukemProject.ViewModel.Resources
{
    public class Validator
    {
        private static Validator instance;

        private Validator() { }

        public static Validator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Validator();
                }
                return instance;
            }
        }

        public bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^((\(?\+45\)?)?)(\s?\d{2}\s?\d{2}\s?\d{2}\s?\d{2})$").Success;
        }

    }
}
