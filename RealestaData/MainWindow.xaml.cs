
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
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
        private bool isDownloading = false; // Flaga oznaczająca stan pobierania danych
        private CancellationTokenSource cancellationTokenSource; // Obiekt do anulowania zadania
        public MainWindow()
        {
            InitializeComponent();
            //Players = new List<RealestaPlayerModel>();
            //List_Of_Top_Players_Dg.ItemsSource = Players;
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
                //Download_Data_Btn.IsEnabled = false;
                InitializeTimer();
                StatusLabel.Foreground = new SolidColorBrush(Colors.Green);
                StatusLabel.Content = "Working ...";

                cancellationTokenSource = new CancellationTokenSource(); // Inicjalizacja CancellationTokenSource

                try
                {   //getting players list
                    await Task.Run(() =>
                    {
                        var realestaScrapper = new RealestaScrapper();
                        Deaths = realestaScrapper.GetDeaths();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            List_Of_Deaths_Dg.ItemsSource = Deaths;

                        });
                        //cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    }, cancellationTokenSource.Token);

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
                        
                        foreach (var Player in Players)
                        {

                            Player.Status = realestaScrapper.GetStatus(Player.Href);
                            Thread.Sleep(1000);
                            PlayersObservableCollection = new ObservableCollection<RealestaPlayerModel>(Players);
                           
                            //cancellationTokenSource.Token.ThrowIfCancellationRequested();
                        }

                        //cancellationTokenSource.Token.ThrowIfCancellationRequested(); // Sprawdzenie czy anulowano zadanie

                        // Tutaj umieść kod pozostałych operacji pobierania danych

                    }, cancellationTokenSource.Token);
                    //geting deaths list
                    
                    //updating players status
                    //await Task.Run(() =>
                    //{
                        


                    //    //foreach (var player in Players)
                    //    //{
                    //    //    player.PropertyChanged += Player_PropertyChanged;
                    //    //}
                    //},cancellationTokenSource.Token);


                }
                catch (OperationCanceledException)
                {
                    // Zadanie zostało anulowane
                }

                PlayersObservableCollection = new ObservableCollection<RealestaPlayerModel>(Players);
                List_Of_Top_Players_Dg.ItemsSource = PlayersObservableCollection;

                //Download_Data_Btn.IsEnabled = true;

            }
            else
            {
                // Anulowanie zadania po drugim naciśnięciu przycisku "Download Data"
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                    isDownloading = false; // Zresetowanie flagi na false - pobieranie danych zakończone
                    StatusLabel.Foreground = new SolidColorBrush(Colors.DarkRed);
                    StatusLabel.Content = "Not working";
                }
            }





        }

        private void List_Of_Top_Players_Dg_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            // Pobranie listy wierszy z DataGrid
            //List<DataGridRow> rows = new List<DataGridRow>();
            //foreach (var item in List_Of_Top_Players_Dg.Items)
            //{
            //    DataGridRow row = (DataGridRow)List_Of_Top_Players_Dg.ItemContainerGenerator.ContainerFromItem(item);
            //    if (row != null)
            //    {
            //        rows.Add(row);
            //    }
            //}

            //// Modyfikacja tła poszczególnych wierszy
            //foreach (var row in rows)
            //{
            //    RealestaPlayerModel player = row.DataContext as RealestaPlayerModel;
            //    if (player != null)
            //    {
            //        if (player.Status == "online")
            //        {
            //            row.Background = new SolidColorBrush(Colors.Green);
            //        }
            //        else if (player.Status == "offline")
            //        {
            //            row.Background = new SolidColorBrush(Colors.Red);
            //        }
            //        else
            //        {
            //            row.Background = new SolidColorBrush(Colors.Gray);
            //        }
            //    }
            



            // Get object connected to row
            //RealestaPlayerModel item = e.Row.DataContext as RealestaPlayerModel;
            //if (item != null)
            //{
            //    // Check Status field. Set the color for online/offline players
            //    Color backgroundColor;
            //    if (item.Status == "unknown")
            //        backgroundColor = Color.FromArgb(216, 255, 0, 0);  // red with 85% transparrency
            //    else if (item.Status == "online")
            //        backgroundColor = Color.FromArgb(216, 0, 255, 0);  // green with 85% transparrency
            //    else
            //        return;

            //    // Crate new brush with new color
            //    var backgroundBrush = new SolidColorBrush(backgroundColor);
            //    e.Row.Background = backgroundBrush;
        //}
        }

    }
}


