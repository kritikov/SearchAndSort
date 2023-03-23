using SearchAndSort.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SearchAndSort.Classes
{
    public class State
    {
        #region VARIABLES

        public List<int> Numbers { get; set; } = new List<int>();
        public string DisplayValue
        {
            get { return this.ToString(); }
        }
        public int N
        {
            get { return Numbers.Count; }
        }
        public int SplitIndex = 0;
        public State? Parent = null;
        public int f = 0;
        public int g = 0;
        public int h = 0;
        public int Weight = 0;

        #endregion


        #region CONSTRUCTORS

        public State()
        {
        }

        public State(string numbersString)
        {
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

        /// <summary>
        /// Create a new state that is child of this state. The child has an index that splits its list of numbers at two parts.
        /// </summary>
        /// <param name="splitIndex"></param>
        /// <returns></returns>
        public State GetChild(int splitIndex)
        {
            State childState = new State();
            childState.Parent = this;
            childState.SplitIndex = splitIndex;
            childState.Weight = 1;

            // calculate costs
            childState.g = childState.Parent.g + childState.Weight;
            childState.f = childState.g + childState.h;

            try
            {
                // split the list of numbers in two parts
                List<int> listLeft = new List<int>();
                List<int> listRight = new List<int>();
                for (int i = 0; i < this.Numbers.Count; i++)
                {
                    if (i <= splitIndex)
                        listLeft.Add(this.Numbers[i]);
                    else
                        listRight.Add(this.Numbers[i]);
                }

                // reverse the numbers in the left list
                listLeft.Reverse();

                // join the left list with the right list
                listLeft.AddRange(listRight);

                // set the numbers of the child as the result
                childState.Numbers = listLeft;
            }
            catch(Exception ex)
            {
                throw;
            }

            return childState;
        }

        /// <summary>
        /// Check if the numbers in the list are sorted ascending.
        /// </summary>
        /// <returns></returns>
        public bool IsSorted()
        {
            try
            {
                int? previousNumber = null;

                // if any number is less than the previious number then the list is not sorted
                foreach (var number in Numbers)
                {
                    if (previousNumber == null)
                    {
                        previousNumber = number;
                        continue;
                    }
                    else
                    {
                        if (previousNumber > number)
                            return false;

                        previousNumber = number;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Check if a state has not an equal anchestor and is unique
        /// </summary>
        /// <returns></returns>
        public bool IsUniqueDescendant()
        {
            State? parent = this.Parent;

            while (parent != null)
            {
                if (parent.DisplayValue == this.DisplayValue)
                    return false;

                parent = parent.Parent;
            }

            return true;
        }

        /// <summary>
        /// Return the best state from a list of states
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
        public static State? GetBestState(List<State> states)
        {
            State? bestState;

            try
            {
                if (states.Count == 0)
                    return null;
                else
                {
                    bestState = states[0];
                    foreach(var state in states)
                    {
                        if (state.f < bestState.f)
                            bestState = state;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return bestState;
        }

        /// <summary>
        /// Analyze a state using the UCS algorithm
        /// </summary>
        public static void UCSAnalysis(State initialState, CancellationToken cancellationToken, MainWindow window)
        {
            try
            {
                List<State> openStates = new List<State>();
                State finalState = null;

                openStates.Add(initialState);
                State selectedState = initialState;

                //Logs.Write("");
                //Logs.Write($"**** Analyzing initial state {initialState.DisplayValue} with UCS algorithm ****");
                //Logs.Write($"Initial state {initialState.DisplayValue}, g={initialState.g}, h={initialState.h}, f={initialState.f}");
                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //});

                // if the initial state is sorted then is the state we search
                if (initialState.IsSorted())
                    finalState = initialState;

                while (finalState == null && openStates.Count != 0)
                {
                    // stop the process if the user has cancel it
                    cancellationToken.ThrowIfCancellationRequested();

                    // open the childs of the selected state
                    for (int i = 1; i < selectedState.N; i++)
                    {
                        State childState = selectedState.GetChild(i);

                        if (childState.IsUniqueDescendant())
                        {
                            openStates.Add(childState);
                            //Logs.Write($"open child {childState.DisplayValue}, g={childState.g}, h={childState.h}, f={childState.f}");
                        }
                    }

                    // remove the selected state from the open states
                    openStates.Remove(selectedState);
                    //Application.Current.Dispatcher.Invoke(() =>
                    //{
                    //    Logs.Write($"closed state {selectedState?.DisplayValue}");
                    //});

                    // keep the unique open states with the best f
                    openStates = openStates.GroupBy(state => state.DisplayValue)
                        .Select(state => state.OrderBy(state => state.f)
                        .First())
                        .ToList();

                    // find the open state with the best f
                    selectedState = GetBestState(openStates);
                    //Logs.Write($"new selected state {selectedState?.DisplayValue}, g={selectedState?.g}, h={selectedState?.h}, f={selectedState?.f}");

                    if (selectedState.IsSorted())
                        finalState = selectedState;
                }

                //Logs.Write($"Analyzing initial state {initialState.DisplayValue} with UCS algorithm ended");
                //Logs.Write("");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        #endregion
    }
}
