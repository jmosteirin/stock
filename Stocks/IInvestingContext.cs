using Stocks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks
{
    public interface IInvestingContext
    {
        IEnumerable<Candle> GetCandles(int paramCount = 70, EIndex paramIndex = EIndex.Ibex35, int paramIntervalInSeconds = 86400);
        IEnumerable<Candle> GetCandlesEx(EIndex paramIndex = EIndex.Ibex35);
    }
}
