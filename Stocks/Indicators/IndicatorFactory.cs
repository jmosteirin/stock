using Stocks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Indicators
{
    public static class IndicatorFactory
    {
        public static IIndicator Build(string paramIndicatorName)
        {
            switch (paramIndicatorName)
            {
                case Constants.Bollinger:
                    return new Indicator(Constants.Bollinger,
                                         (paramMidPoints, paramHighPoints, paramLowPoints, p) => paramMidPoints.BollingerIndicatorBeforeClipping(p[EParameter.ClipValue]).Invert().Clip(1.0),
                                         new Tuple<EParameter, double>[] { new Tuple<Entities.EParameter, double>(EParameter.ClipValue, 1.0) });
                case Constants.MACD:
                    return new Indicator(Constants.MACD,
                                         (paramMidPoints, paramHighPoints, paramLowPoints, p) => paramMidPoints.MACDHistogram().SellingBuyingSignal(p[EParameter.DecayFactor]),
                                         new Tuple<EParameter, double>[] { new Tuple<Entities.EParameter, double>(EParameter.DecayFactor, 0.2) });
                case Constants.RSI:
                    return new Indicator(Constants.RSI,
                                         (paramMidPoints, paramHighPoints, paramLowPoints, p) =>
                                            paramHighPoints.Subtract(paramLowPoints).
                                            RSI(14).
                                            Transform(sam => sam.Multiply(new Sample(0.02, sam.Date)).
                                            Subtract(new Sample(1.0, sam.Date))),
                                         new Tuple<EParameter, double>[0]);
                case Constants.Stability:
                    return new Indicator(Constants.Stability,
                                         (paramMidPoints, paramHighPoints, paramLowPoints, p) =>
                                            paramMidPoints.StandardDeviation(20).
                                            Divide(paramMidPoints.Average(20)).
                                            Transform(s => new Sample(1.0, s.Date).Subtract(s)).Clip(1),
                                         new Tuple<EParameter, double>[0]);
                default:
                    throw new ArgumentException(@"paramIndicatorName");
            }
        }
    }
}
