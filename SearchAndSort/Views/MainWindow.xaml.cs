using SearchAndSort.Classes;
using SearchAndSort.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

        private bool SearchingValuationsRunning = false;
        private CancellationTokenSource cancellationToken = new CancellationTokenSource();

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

        #endregion


        #region COMMANDS

        private void NewState_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (SearchingValuationsRunning == false) ? true : false;

        }
        private void NewState_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Message = "";

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

        private void RandomState_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (SearchingValuationsRunning == false) ? true : false;

        }
        private void RandomState_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Message = "";

            try
            {
                RandomState();
            }
            catch (Exception ex)
            {
                Logs.Write(ex.Message);
                Message = ex.Message;
            }
        }

        private void UCSAnalysis_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (InitialState != null && SearchingValuationsRunning == false) ? true : false;

        }
        private void UCSAnalysis_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Message = "";

            try
            {
                SearchingValuationsRunning = true;
                UCSAnalysis();
            }
            catch (Exception ex)
            {
                Logs.Write(ex.Message);
                Message = ex.Message;
            }
        }

        private void ASTARAnalysis_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (InitialState != null && SearchingValuationsRunning == false) ? true : false;

        }
        private void ASTARAnalysis_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Message = "";

            try
            {
                //SearchingValuationsRunning = true;
                //UCSAnalysis();
            }
            catch (Exception ex)
            {
                Logs.Write(ex.Message);
                Message = ex.Message;
            }
        }

        private void StopAnalysis_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (SearchingValuationsRunning == true) ? true : false;

        }
        private void StopAnalysis_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Message = "";

            try
            {
                cancellationToken.Cancel();
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

        /// <summary>
        /// Display a window to create a new random state
        /// </summary>
        private void RandomState()
        {
            try
            {
                RandomStateWindow window = new RandomStateWindow();
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

        private async Task UCSAnalysis()
        {
            try
            {
                //RefreshSolverViews();

                cancellationToken = new CancellationTokenSource();

                await Task.Run(() => {
                    State.UCSAnalysis(InitialState, cancellationToken.Token, this);
                });

                SearchingValuationsRunning = false;
                //RefreshSolverViews();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SearchingValuationsRunning = false;
            }
        }


        #endregion

        
    }
}




