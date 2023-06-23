
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;



namespace RealestaData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        internal List<RealestaPlayerModel> Players { get; set; }
        internal List<RealestaDeathsModel> Deaths { get; set; }
        private Timer reloadTimer;
        private bool isFirstExecution = true;
        public MainWindow()
        {
            InitializeComponent();
        
        }
        
        private bool isOnline = false;

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
            reloadTimer.Interval = 60000; // time interval in miliseconds (60000 ms = 1 min)
            reloadTimer.Elapsed += ReloadTimerElapsed;
            reloadTimer.Start();
        }

        private async void Download_Data_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (isFirstExecution)
            {
                isFirstExecution = false;
                InitializeTimer();
            }

            await Task.Run(() =>
            {
                var realestaScrapper = new RealestaScrapper();
                Players = realestaScrapper.GetPlayers();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    List_Of_Top_Players_Dg.ItemsSource = Players;
                    
                });
                
            });
            await Task.Run(() =>
            {
                var realestaScrapper = new RealestaScrapper();
                Deaths = realestaScrapper.GetDeaths();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    List_Of_Deaths_Dg.ItemsSource = Deaths;

                });

            });

           
        }
       
    }
}

// MOST COMMON ERROR DURING OPERATION ON INTERNETDATA 
// MOST COMMON ERROR HANDLING / ABORTING ETC.
// INNE DATA ONLINE - GREEN, OFFLINE - RED 
// GUILD 
//skonczyć program do piątku - realesta74 startuje!