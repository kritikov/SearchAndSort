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

        /// <summary>
        /// Check if a string if is valid to create a state
        /// </summary>
        /// <param name="stateString"></param>
        /// <returns></returns>
        public static bool ValidateStateString(string stateString)
        {

            // check for unique numbers
            var numbers = new List<int>();
            var numbersArray = stateString.Split(new string[] { "," }, StringSplitOptions.None);
            foreach (var number in numbersArray)
            {
                var num = Convert.ToInt32(number.Trim());
                if (numbers.Contains(num))
                    return false;
                else
                    numbers.Add(num);
            }

            return true;
        }

        /// <summary>
        /// Create a state with random values
        /// </summary>
        /// <param name="numbersCount"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static State RandomState(int numbersCount, int min, int max)
        {
            State state = new State();
            Random random = new Random();

            int i = 1;
            int invalidAttempts = 0;

            while (i<= numbersCount)
            {
                int number = random.Next(min, max);

                if (state.Numbers.Contains(number))
                {
                    invalidAttempts++;

                    if (invalidAttempts == 100)
                        throw new Exception("Unable to create a list with random numbers. Maybe you should give bigger internal for numbers and less length of the array");

                    continue;
                }
                else
                {
                    state.Numbers.Add(number);
                    i++;
                }
            }

            return state;
        }

        #endregion
    }
}
