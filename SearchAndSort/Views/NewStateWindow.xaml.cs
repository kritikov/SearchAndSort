using SearchAndSort.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SearchAndSort.Views
{
    /// <summary>
    /// Interaction logic for NewState.xaml
    /// </summary>
    public partial class NewStateWindow : Window, INotifyPropertyChanged
    {
        #region VARIABLES

        private string message = "";
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
            }
        }

        private string stateString = "1, 2, 3, 4, 5";
        public string StateString
        {
            get => stateString;
            set
            {
                stateString = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StateString)));
            }
        }

        private string resultStateString = "";
        public string ResultStateString
        {
            get => resultStateString;
            set
            {
                resultStateString = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ResultStateString)));
            }
        }

        public State StateCreated { get; set; }

        #endregion


        #region CONSTRUCTORS

        public NewStateWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        #endregion


        #region EVENTS

        public event PropertyChangedEventHandler PropertyChanged;

        private void CreateState(object sender, RoutedEventArgs e)
        {
            Message = "";
            try
            {
                State.ValidateStateString(StateString);
                StateCreated = new State(StateString);
                Close();
            }
            catch (Exception ex)
            {
                Logs.Write(ex.Message);
                Message = ex.Message;
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Message = "";
            StateString = "";
            Close();
        }

        

        #endregion



    }
}
