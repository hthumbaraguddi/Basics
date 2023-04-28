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
    }
}
