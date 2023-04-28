using System;
using System.Collections.Concurrent;
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

    interface IStockService
    {
        IAsyncEnumerable<StockOHLC> GetAllStocksStreams(IProgress<int> progress);
    }
    public class StocksDataStreamingService : IStockService
    {
        string filePath = "Data\\INFY.NS.csv";

        public async IAsyncEnumerable<StockOHLC> GetAllStocksStreams(IProgress<int> progress)
        {
            using var stream = new StreamReader(File.OpenRead(filePath));
           
            await stream.ReadLineAsync();

            int i = 0;

            while (await stream.ReadLineAsync() is string line)
            {
                i++;
                progress.Report(i);
                await Task.Delay(5);
                yield return StockOHLC.FromText(line);
            }
        }

        public static ConcurrentBag<StockOHLC> GenerateStocks(string symbol)
        {
            var data = new ConcurrentBag<StockOHLC>();

            for(int i =0;i<5000000;i++)
            {
                var s = new StockOHLC()
                {
                    Symbol = symbol,
                    Date = "23-04-2023",
                    Close = new Random().Next(1200, 1250),
                    High = new Random().Next(1200, 1250),
                    Low = new Random().Next(1200, 1250),
                    Open = new Random().Next(1200, 1250)
                };

                data.Add(s);
            }
            return data;
        }
    }
}
