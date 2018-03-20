using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Indicators
{
    public interface IIndicator
    {
        string GetName();
        int GetExtraDimensionCount();
        void SetExtraDimension(int paramDimension, double paramDimensionValue);
        IEnumerable<Sample> GetValue(IEnumerable<Sample> paramMidPoints, IEnumerable<Sample> paramHighPoints, IEnumerable<Sample> paramLowPoints);
        string GetDimensionName(int paramDimension);
    }
}
