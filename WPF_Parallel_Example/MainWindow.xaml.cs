using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
using TASK_TPL_WPF;
using TASK_TPL_WPF.Domain;

namespace WPF_Parallel_Example
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

        private void BlockingCallingThread_Click(object sender, RoutedEventArgs e)
        {
            ConcurrentBag<StockOHLC> stockOHLCs = new ConcurrentBag<StockOHLC>();
            ConcurrentBag<string> strings = new ConcurrentBag<string>();
            Parallel.Invoke(

                () =>
                {
                    ConcurrentBag<StockOHLC> s = StocksDataStreamingService.GenerateStocks("INFY");
                    s.AsEnumerable().ToList().ForEach(stockOHLC =>
                    {
                        stockOHLCs.Add(stockOHLC);
                    });

                    strings.Add($"INFY: {Thread.CurrentThread.ManagedThreadId.ToString()}");

                },

                () =>
                {
                    ConcurrentBag<StockOHLC> s = StocksDataStreamingService.GenerateStocks("HMS");
                    s.AsEnumerable().ToList().ForEach(stockOHLC =>
                    {
                        stockOHLCs.Add(stockOHLC);
                    });

                    strings.Add($"HMS: {Thread.CurrentThread.ManagedThreadId.ToString()}");

                },
                 () =>
                 {
                     ConcurrentBag<StockOHLC> s = StocksDataStreamingService.GenerateStocks("TCS");
                     s.AsEnumerable().ToList().ForEach(stockOHLC =>
                     {
                         stockOHLCs.Add(stockOHLC);
                     });
                     strings.Add($"TCS: {Thread.CurrentThread.ManagedThreadId.ToString()}");
                 }
                );

            this.stocksGrid.ItemsSource = stockOHLCs;
            this.ErrorDisplayer.Text = String.Join("\n", strings);
        }       
    }
}
