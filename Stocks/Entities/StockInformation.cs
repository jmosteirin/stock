﻿using Stocks.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Entities
{
    public class StockInformation
    {
        public IEnumerable<StockInformationForIndex> StockInformationForIndexes { get; }

        public StockInformation(params StockInformationForIndex[] paramStockInformationForIndexes)
        {
            StockInformationForIndexes = paramStockInformationForIndexes;
        }
    }
    public class StockInformationForIndex
    {
        public int Index { get; set; }
        public IEnumerable<Candle> Candles { get; set; }
        public IEnumerable<Sample> LowPoints { get => Candles.Select(c => new Sample() { Date = c.Date, Valid = true, Value = c.Low }).ToArray(); }
        public IEnumerable<Sample> HighPoints { get => Candles.Select(c => new Sample() { Date = c.Date, Valid = true, Value = c.High }).ToArray(); }
        public IEnumerable<Sample> MidPoints { get => Candles.Select(c => new Sample() { Date = c.Date, Valid = true, Value = (c.High + c.Low) / 2.0 }).ToArray(); }

        public StockInformationForIndex(int paramIndex, IEnumerable<Candle> paramCandles)
        {
            Index = paramIndex;
            Candles = paramCandles;
        }
    }
}
