using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using TASK_TPL_WPF.Domain;

namespace TASK_TPL_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StocksDataSync stocksDataSync = new StocksDataSync();
            
            this.stocksGrid.ItemsSource = stocksDataSync.ReadStocksData();
        }

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            StocksDataSync stocksDataSync = new StocksDataSync();

            stocksDataSync.ReadStocksDataHybridAsync();            

            this.stocksGrid.ItemsSource = stocksDataSync.StockOHLCs;
        }

        private void Button_Click_CheckForException(object sender, RoutedEventArgs e)
        {
            ReadStocksDataHybridAsync_WithExceptionCapture();
        }
        public void ReadStocksDataHybridAsync_WithExceptionCapture()
        {

            //using async and await within the Task
            var Stocks = new ObservableCollection<StockOHLC>();
            var dataTask = Task.Run(async () =>
            {
                using var stream = new StreamReader(File.OpenRead("SomeFile_which is not availabe"));

                var allLineData = new List<string>();
                while (await stream.ReadLineAsync() is string line)
                {
                    allLineData.Add(line);
                }
                MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());

                return allLineData;
            });

            //Just to check if above task produced any error. In this case, we have introduced an exception
            dataTask.ContinueWith(t =>
            {
                //Calling Dispatcher.Invoke as Task is on different thread than UI thread
                Dispatcher.Invoke(() =>
                {
                    this.ErrorDisplayer.Text = t.Exception?.Message;
                });

            }, TaskContinuationOptions.OnlyOnFaulted);

            var processLinesTask = dataTask.ContinueWith((JustCompletedTask) =>
            {
                var infyData = JustCompletedTask.Result;

                foreach (var inf in infyData.Skip(1))
                {
                    if (!inf.Contains("null"))
                    {
                        Stocks.Add(StockOHLC.FromText(inf));
                    }
                }
                MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());

                Dispatcher.Invoke(() =>
                {
                    this.stocksGrid.ItemsSource = Stocks;
                });

            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            

            MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());

        }

        CancellationTokenSource? cancellationTokenSource;
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
                this.SearchButton.Content = "Search";
            }

            cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.Token.Register(() =>
            {
                this.ErrorDisplayer.Text = "Cancellation Requested";
            });

            SearchButton.Content = "Cancel";


            var Stocks = new ObservableCollection<StockOHLC>();
            Task<IEnumerable<string>> dataTask = LoadAllData(cancellationTokenSource);

            var processLinesTask = dataTask.ContinueWith((JustCompletedTask) =>
            {
                var infyData = JustCompletedTask.Result;

                foreach (var inf in infyData)
                {
                    if (cancellationTokenSource.IsCancellationRequested) { break; }

                    if (!inf.Contains("null"))
                    {
                        Stocks.Add(StockOHLC.FromText(inf));
                    }
                }

                return Stocks;
            }, cancellationTokenSource.Token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Current);

            this.stocksGrid.ItemsSource = processLinesTask.Result;

            // SearchButton.Content = "Search";

        }

        private Task<IEnumerable<string>> LoadAllData(CancellationTokenSource cancellationTokenSource)
        {
            return Task.Run(async () =>
            {
                using var stream = new StreamReader(File.OpenRead("Data\\INFY.NS.csv"));

                var allLineData = new List<string>();

                while (await stream.ReadLineAsync() is string line)
                {
                    if (cancellationTokenSource.IsCancellationRequested) { break; }
                    allLineData.Add(line);
                }

                return allLineData.Skip(1);
            }, cancellationTokenSource.Token);
        }
    }
}
