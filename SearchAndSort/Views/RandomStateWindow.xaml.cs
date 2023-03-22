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
    /// Interaction logic for RandomStateWindow.xaml
    /// </summary>
    public partial class RandomStateWindow : Window, INotifyPropertyChanged
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

        public int Length { get; set; } = 5; // from 8 and after is very slow
        
        public int MinimumNumber { get; set; } = 0;
        
        public int MaximumNumber { get; set; } = 1000;

        public State StateCreated { get; set; }

        #endregion


        #region EVENTS

        public event PropertyChangedEventHandler PropertyChanged;

        private void CreateState(object sender, RoutedEventArgs e)
        {
            Message = "";
            try
            {
                StateCreated = State.RandomState(Length, MinimumNumber, MaximumNumber);
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
            Close();
        }

        #endregion


        #region CONSTRUCTORS

        public RandomStateWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        #endregion
    }
}
