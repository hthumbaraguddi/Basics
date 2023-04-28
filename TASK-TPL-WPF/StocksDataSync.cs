using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using TASK_TPL_WPF.Domain;

namespace TASK_TPL_WPF
{
    public class StocksDataSync
    {
        string filePath = "Data\\INFY.NS.csv";

        public ObservableCollection<StockOHLC> StockOHLCs
        {
            get; set;
        }

        public StocksDataSync()
        {
            StockOHLCs = new ObservableCollection<StockOHLC>();
        }
        public ObservableCollection<StockOHLC> ReadStocksData()
        {
            var infyData = File.ReadAllLines(filePath);
            foreach (var inf in infyData.Skip(1))
            {
                if (!inf.Contains("null"))
                {
                    StockOHLCs.Add(StockOHLC.FromText(inf));
                    Thread.Sleep(1000);
                }
            }

            return StockOHLCs;
        }

        public void ReadStocksDataHybridAsync()
        {
            var Stocks = new ObservableCollection<StockOHLC>();
            var dataTask = Task.Run(() =>
            {
                var infyData = File.ReadAllLines(filePath);
                MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());
                return infyData.Skip(1);
            });

            var processLinesTask = dataTask.ContinueWith((JustCompletedTask) =>
            {
                var infyData = JustCompletedTask.Result;

                foreach (var inf in infyData)
                {
                    if (!inf.Contains("null"))
                    {

                        Stocks.Add(StockOHLC.FromText(inf));
                        
                    }
                }
                MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());
                return Stocks;
            });

          this.StockOHLCs=  processLinesTask.Result;
            MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());

        }

        public void ReadStocksDataHybridAsync_2()
        {

            //using async and await within the Task
            var Stocks = new ObservableCollection<StockOHLC>();
            var dataTask = Task.Run(async () =>
            {
                using var stream = new StreamReader(File.OpenRead(filePath));

                var allLineData = new List<string>();
                while( await stream.ReadLineAsync() is string line)
                {
                    allLineData.Add(line);
                }
                MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());

                return allLineData;
            });

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
                return Stocks;
            });

            this.StockOHLCs = processLinesTask.Result;

            MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());

        }

      
    }
}
