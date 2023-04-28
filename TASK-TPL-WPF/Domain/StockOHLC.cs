using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASK_TPL_WPF.Domain
{
    public class StockOHLC
    {
        public string Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }

        public static StockOHLC FromText(string text)
        {
            var ohlc = text.Split(",");
            StockOHLC stockOHLC = new StockOHLC()
            {
                Date = ohlc[0],
                Open = Convert.ToDouble(ohlc[1]),
                High = Convert.ToDouble(ohlc[2]),
                Low = Convert.ToDouble(ohlc[3]),
                Close = Convert.ToDouble(ohlc[4]),
                Volume = Convert.ToDouble(ohlc[5])
            };

            return stockOHLC;
        }

    }
}
