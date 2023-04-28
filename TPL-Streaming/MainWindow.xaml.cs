using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TASK_TPL_WPF;
using TASK_TPL_WPF.Domain;

namespace TPL_Streaming
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<int>();
            
            progress.ProgressChanged += (_, p) =>
            {
                this.status.Text = p.ToString();
                StatusProgress.Value = (p * 100 / 6501);
            };
            StocksDataStreamingService stocksDataStreamingService = new StocksDataStreamingService();

            var data =stocksDataStreamingService.GetAllStocksStreams(progress);
                      

            var ObCollection = new ObservableCollection<StockOHLC>();
            this.stocksGrid.ItemsSource = ObCollection;

           await foreach (var stock in data)
           {
                ObCollection.Add(stock);
           }
           
        }
    }
}
