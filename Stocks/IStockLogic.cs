using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks
{
    public interface IStockLogic
    {
        void RefreshStoredIndexes();
        void LetsBecomeRich();
        void ExportCSV(string paramFileName);
        void AddIndexesToCache(int paramStartId, int paramEndId);
    }
}
