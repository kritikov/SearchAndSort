using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAndSort.Classes
{
    public class State
    {
        #region VARIABLES

        public List<int> Numbers { get; set; }

        public string DisplayValue => this.ToString();

        public int N => Numbers.Count;

        #endregion


        #region CONSTRUCTORS

        public State()
        {
            Numbers = new List<int>();
        }

        public State(string numbersString)
        {
            Numbers = new List<int>();

            var numbers = numbersString.Split(new string[] { "," }, StringSplitOptions.None);

            foreach (var number in numbers)
            {
                Numbers.Add(Convert.ToInt32(number.Trim()));
            }
        }

        #endregion


        #region METHODS

        public override string ToString()
        {
            string result = "";

            foreach (var number in Numbers)
            {
                if (result != "")
                    result += ", ";

                result += number.ToString();
            }

            result = $"[{result}]";

            return result;
        }

        #endregion
    }
}
