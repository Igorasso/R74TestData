
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using Timer = System.Timers.Timer;


namespace RealestaData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        internal List<RealestaPlayerModel> Players { get; set; }
        internal ObservableCollection<RealestaPlayerModel> PlayersObservableCollection { get; set; }
        internal List<RealestaDeathsModel> Deaths { get; set; }
        private Timer reloadTimer;
        private bool isFirstExecution = true;
        private bool isDownloading = false; // Download data state flag 
        private CancellationTokenSource cancellationTokenSource; // task canceling object
        public MainWindow()
        {
            InitializeComponent();
            PlayersObservableCollection = new ObservableCollection<RealestaPlayerModel>();
        }
        private async void ReloadTimerElapsed(object sender, ElapsedEventArgs e)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                Download_Data_Btn_Click(null, null);
            });
        }
        private void InitializeTimer()
        {
            reloadTimer = new Timer();
            reloadTimer.Interval = 60000; // time interval in miliseconds 
            reloadTimer.Elapsed += ReloadTimerElapsed;
            reloadTimer.Start();
        }
        private async void Download_Data_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (!isDownloading)
            {
                isDownloading = true; // Ustawienie flagi na true - rozpoczęcie pobierania danych
                InitializeTimer();
                StatusLabel.Foreground = new SolidColorBrush(Colors.Green);
                StatusLabel.Content = "Working ...";
                cancellationTokenSource = new CancellationTokenSource(); // Inicjalizacja CancellationTokenSource
                try
                {   // Getting deaths list
                    await Task.Run(() =>
                    {
                        var realestaScrapper = new RealestaScrapper();
                        Deaths = realestaScrapper.GetDeaths();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            List_Of_Deaths_Dg.ItemsSource = Deaths;

                        });
                    }, cancellationTokenSource.Token);
                    // Getting players list
                    await Task.Run(() =>
                    {
                        var realestaScrapper = new RealestaScrapper();
                        Players = realestaScrapper.GetPlayers();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            PlayersObservableCollection = new ObservableCollection<RealestaPlayerModel>(Players);
                            List_Of_Top_Players_Dg.ItemsSource = PlayersObservableCollection;
                            //List_Of_Top_Players_Dg.ItemsSource = Players;
                        });
                        // Updating Player Status 
                        foreach (var Player in Players)
                        {
                            
                            //Player.Status = realestaScrapper.GetStatus(Player.Href);
                            //Thread.Sleep(1000);
                            //PlayersObservableCollection = new ObservableCollection<RealestaPlayerModel>(Players);
                        }
                    }, cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    // Task canceled
                }
                foreach (var Player in Players)
                {

                    //Player.Status = realestaScrapper.GetStatus(Player.Href);
                    //Thread.Sleep(1000);
                    //PlayersObservableCollection = new ObservableCollection<RealestaPlayerModel>(Players);
                }
                PlayersObservableCollection = new ObservableCollection<RealestaPlayerModel>(Players);
                List_Of_Top_Players_Dg.ItemsSource = PlayersObservableCollection;
            }
            else
            {
                // Start/Stop like cancelation 
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                    isDownloading = false; // flag reset to false - downloading data finished, need a some polishing
                    StatusLabel.Foreground = new SolidColorBrush(Colors.DarkRed);
                    StatusLabel.Content = "Not working";
                }
            }

        }

        private void Test_Btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var Player in PlayersObservableCollection)
            {
                if (Player.Status == "online")
                {
                    Player.Status = "offline";
                }
                else
                {
                    Player.Status = "online";
                }
            }
        }
    }
}


