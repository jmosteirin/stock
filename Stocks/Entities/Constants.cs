﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Entities
{
    public class Constants
    {
        public const string Bollinger = @"Bollinger";
        public const string MACD = @"MACD";
        public const string RSI = @"RSI";
        public const string Stability = @"Stability";

        public const string IndexesCacheFileName = @"stock-indexes.json";
        public const int StockIndexesStaringId = 238;
        public const int StockIndexesEndingId = 12000;
    }
}
