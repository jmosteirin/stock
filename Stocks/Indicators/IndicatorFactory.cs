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
                case IndicatorConstants.Bollinger:
                    return new Indicator(IndicatorConstants.Bollinger,
                                         (paramMidPoints, paramHighPoints, paramLowPoints, p) => paramMidPoints.BollingerIndicatorBeforeClipping(p[EParameter.ClipValue]).Invert().Clip(1.0),
                                         new Tuple<EParameter, double>[] { new Tuple<Entities.EParameter, double>(EParameter.ClipValue, 1.0) });
                case IndicatorConstants.MACD:
                    return new Indicator(IndicatorConstants.MACD,
                                         (paramMidPoints, paramHighPoints, paramLowPoints, p) => paramMidPoints.MACDHistogram().SellingBuyingSignal(p[EParameter.DecayFactor]),
                                         new Tuple<EParameter, double>[] { new Tuple<Entities.EParameter, double>(EParameter.DecayFactor, 0.2) });
                case IndicatorConstants.RSI:
                    return new Indicator(IndicatorConstants.RSI,
                                         (paramMidPoints, paramHighPoints, paramLowPoints, p) => 
                                            paramHighPoints.Subtract(paramLowPoints).
                                            RSI(14).
                                            Transform(sam => sam.Multiply(new SingleValueSample<double>(NumericWrapper<double>.Build(2.0), sam.Date)).
                                            Subtract(new SingleValueSample<double>(NumericWrapper<double>.Build(1.0), sam.Date))),
                                         new Tuple<EParameter, double>[0]);
                default:
                    throw new ArgumentException(@"paramIndicatorName");
            }
        }
    }
}
