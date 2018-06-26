using Stocks.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stocks.Entities;

namespace Stocks
{
    public interface IStockLogic
    {
        void RefreshStoredIndexes();
        EvaluationStep LetsBecomeRich(int paramCurrentIndex = 0);
        void ExportCSV(string paramFileName);
        void AddIndexesToCache(int paramStartId, int paramEndId);
        IEnumerable<Sample> Eval(int paramIndex, double paramBollinger, double paramMACD, double paramRSI);
        IEnumerable<Sample> EvalIndicator(string paramIndicator, int paramIndex);
    }
}
