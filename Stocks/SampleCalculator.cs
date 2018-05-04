using Stocks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks
{
    public abstract class SampleCalculator<TSample>
        where TSample : Sample
    {
        public abstract TSample BuildInvalid(TSample paramSample);
        public abstract TSample Clone(TSample paramSample);
        public abstract TSample GetZero(TSample paramSample);
        public abstract TSample GetUnit(TSample paramSample);
        public abstract double Compare(TSample paramFirstSample, TSample paramSecondSample);
        public abstract TSample Add(TSample paramSample, params TSample[] paramSamples);
        public abstract TSample Subtract(TSample paramFirstSample, TSample paramSecondSample);
        public abstract TSample Divide(TSample paramFirstSample, TSample paramSecondSample);
        public abstract TSample ScalarMultiply(TSample paramSample, double paramScalar);
        public abstract TSample Pow(TSample paramSample, double paramScalar);
        public TSample Average(TSample paramSample, params TSample[] paramSamples)
        {
            if (!paramSamples.Any())
                throw new Exception(Constants.EmptyCollection);

            return ScalarMultiply(Add(paramSample, paramSamples), 1.0 / paramSamples.Count());
        }

        public TSample Negate(TSample paramSample)
        {
            return ScalarMultiply(paramSample, -1.0);
        }

        public abstract TSample GetValue(TSample paramSample, double paramClipValue);
    }
}
