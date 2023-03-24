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

        private bool searchingValuationsRunning = false;
        public bool SearchingValuationsRunning
        {
            get
            {
                return searchingValuationsRunning;
            }
            set
            {
                searchingValuationsRunning = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchingValuationsRunning"));
            }
        }

        private int selectedTab = 0;
        public int SelectedTab
        {
            get { return selectedTab; }
            set
            {
                selectedTab = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedTab"));
            }
        }

        private CancellationTokenSource cancellationToken = new CancellationTokenSource();

        public ObservableCollection<string> Results { get; set; } = new ObservableCollection<string>();

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

            InitialState = new State("3, 5, 4, 6, 1, 2");

            Logs.Write("Application started");
        }

        #endregion


        #region EVENTS

        public event PropertyChangedEventHandler PropertyChanged;

        private void ExitProgram(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ClearLogs(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearLogs();
            }
            catch (Exception ex)
            {
                Logs.Write(ex.Message);
                Message = ex.Message;
            }
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
                CommandManager.InvalidateRequerySuggested();

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
                ASTARAnalysis();
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

        private void ClearLogs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (SearchingValuationsRunning == false) ? true : false;

        }
        private void ClearLogs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Message = "";

            try
            {
                ClearLogs();
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
            resultsSource.Source = Results;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ResultsView)));
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

        /// <summary>
        /// Solve the initial state using the UCS algorithm
        /// </summary>
        /// <returns></returns>
        private async Task UCSAnalysis()
        {
            try
            {
                cancellationToken = new CancellationTokenSource();

                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Results.Clear();
                        RefreshViews();
                    });

                    Results = State.UCSAnalysis(InitialState, cancellationToken.Token);
                });
            }
            catch (Exception ex)
            {
                Logs.Write(ex.Message);
                Message = ex.Message;
            }
            finally
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SearchingValuationsRunning = false;
                    RefreshViews();
                    CommandManager.InvalidateRequerySuggested();
                });
            }
        }

        /// <summary>
        /// Solve the initial state using the A* algorithm
        /// </summary>
        /// <returns></returns>
        private async Task ASTARAnalysis()
        {
            try
            {
                cancellationToken = new CancellationTokenSource();

                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Results.Clear();
                        RefreshViews();
                    });

                    Results = State.ASTARAnalysis(InitialState, cancellationToken.Token);
                });
            }
            catch (Exception ex)
            {
                Logs.Write(ex.Message);
                Message = ex.Message;
            }
            finally
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SearchingValuationsRunning = false;
                    RefreshViews();
                    CommandManager.InvalidateRequerySuggested();
                });
            }
        }

        /// <summary>
        /// Clear the messages from the logs
        /// </summary>
        private void ClearLogs()
        {
            Logs.Clear();
        }

        #endregion

        
    }
}




