using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks
{
    public interface IStockLogic
    {
        void RefreshStoredIndexes(int paramStartId = 0, int paramEndId = 10000);
        void LetsBecomeRich();
    }
}
