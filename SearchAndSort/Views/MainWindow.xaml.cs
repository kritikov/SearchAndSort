using SearchAndSort.Classes;
using SearchAndSort.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SearchAndSort.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region VARIABLES AND NESTED CLASSES

        private string message = "";
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
            }
        }

        private State initialState = new State();
        public State InitialState 
        {
            get { return initialState; }
            set
            {
                initialState = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InitialState"));
            }
        }

        #endregion


        #region VIEWS

        private CollectionViewSource logsSource = new CollectionViewSource();
        public ICollectionView LogsView
        {
            get
            {
                return this.logsSource.View;
            }
        }

        private readonly CollectionViewSource resultsSource = new CollectionViewSource();
        public ICollectionView ResultsView
        {
            get
            {
                return this.resultsSource.View;
            }
        }

        #endregion


        #region CONSTRUCTORS

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            logsSource.Source = Logs.List;

            InitialState = new State("3, 5, 4, 1, 2");

            Logs.Write("Application started");
        }

        #endregion


        #region EVENTS

        public event PropertyChangedEventHandler PropertyChanged;

        private void ExitProgram(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void NewState(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateNewState();
            }
            catch (Exception ex)
            {
                Logs.Write(ex.Message);
                Message = ex.Message;
            }
        }

        #endregion

        #region METHODS

        public void RefreshViews()
        {
            //resultsSource.Source = AK.Clauses;
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ResultsView"));
        }

        /// <summary>
        /// Display a window to create a new initial state
        /// </summary>
        private void CreateNewState()
        {
            try
            {
                NewStateWindow window = new NewStateWindow();
                window.ShowDialog();

                if (window.StateCreated != null)
                {
                    InitialState = window.StateCreated;
                    RefreshViews();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}




