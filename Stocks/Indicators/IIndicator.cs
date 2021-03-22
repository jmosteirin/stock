﻿using Stocks.Entities;
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
        IEnumerable<SingleValueSample<double>> GetValue(IEnumerable<SingleValueSample<double>> paramMidPoints, IEnumerable<SingleValueSample<double>> paramHighPoints, IEnumerable<SingleValueSample<double>> paramLowPoints);
        string GetDimensionName(int paramDimension);
    }
}
