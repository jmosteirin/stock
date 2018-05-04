using Stocks.Calculators;
using Stocks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stocks
{
    public static class SequenceHelper
    {
        private static SequenceManipulator<DoubleSample, SingleValueSampleCalculator<double, DoubleSample>> manipulator = new SequenceManipulator<DoubleSample, SingleValueSampleCalculator<double, DoubleSample>>(new SingleValueSampleCalculator<double, DoubleSample>());
        public static IEnumerable<DoubleSample> BufferedTransform(this IEnumerable<DoubleSample> paramSequence, int paramNumberOfSamples, Func<Queue<DoubleSample>, DoubleSample, DoubleSample> paramTransform)
        {
            return manipulator.BufferedTransform(paramSequence, paramNumberOfSamples, paramTransform);
        }
        public static IEnumerable<DoubleSample> Transform(this IEnumerable<DoubleSample> paramSequence, Func<DoubleSample, DoubleSample> paramTransform)
        {
            return manipulator.Transform(paramSequence, paramTransform);
        }
        public static IEnumerable<DoubleSample> SetToZeroSamplesThatNot(this IEnumerable<DoubleSample> paramSequence, Func<DoubleSample, bool> paramCondition)
        {
            return manipulator.SetToZeroSamplesThatNot(paramSequence, paramCondition);
        }
        public static IEnumerable<DoubleSample> OnlyPositives(this IEnumerable<DoubleSample> paramSequence)
        {
            return manipulator.OnlyPositives(paramSequence);
        }
        public static IEnumerable<DoubleSample> OnlyNegatives(this IEnumerable<DoubleSample> paramSequence)
        {
            return manipulator.OnlyNegatives(paramSequence);
        }
        public static IEnumerable<DoubleSample> Accumulate(this IEnumerable<DoubleSample> paramSequence, int paramNumberOfSamples)
        {
            return manipulator.Accumulate(paramSequence, paramNumberOfSamples);
        }
        public static IEnumerable<DoubleSample> Average(this IEnumerable<DoubleSample> paramSequence, int paramNumberOfSamples)
        {
            return manipulator.Average(paramSequence, paramNumberOfSamples);
        }
        public static IEnumerable<DoubleSample> ExponentialAverage(this IEnumerable<DoubleSample> paramSequence, int paramNumberOfSamples)
        {
            return manipulator.ExponentialAverage(paramSequence, paramNumberOfSamples);
        }
        public static IEnumerable<DoubleSample> Subtract(this IEnumerable<DoubleSample> paramSequence, IEnumerable<DoubleSample> paramSecondSequence)
        {
            return manipulator.Subtract(paramSequence, paramSecondSequence);
        }
        public static IEnumerable<DoubleSample> Add(this IEnumerable<DoubleSample> paramSequence, IEnumerable<DoubleSample> paramSecondSequence)
        {
            return manipulator.Add(paramSequence, paramSecondSequence);
        }
        public static IEnumerable<DoubleSample> Divide(this IEnumerable<DoubleSample> paramSequence, IEnumerable<DoubleSample> paramSecondSequence)
        {
            return manipulator.Divide(paramSequence, paramSecondSequence);
        }
        public static IEnumerable<DoubleSample> ScalarMultiplication(this IEnumerable<DoubleSample> paramSequence, double paramScalar)
        {
            return manipulator.ScalarMultiplication(paramSequence, paramScalar);
        }
        public static IEnumerable<DoubleSample> Square(this IEnumerable<DoubleSample> paramSequence)
        {
            return manipulator.Square(paramSequence);
        }
        public static IEnumerable<DoubleSample> Sqrt(this IEnumerable<DoubleSample> paramSequence)
        {
            return manipulator.Sqrt(paramSequence);
        }
        public static IEnumerable<DoubleSample> Variance(this IEnumerable<DoubleSample> paramSequence, int paramNumberOfSamples)
        {
            return manipulator.Variance(paramSequence, paramNumberOfSamples);
        }
        public static IEnumerable<DoubleSample> StandardDeviation(this IEnumerable<DoubleSample> paramSequence, int paramNumberOfSamples)
        {
            return manipulator.StandardDeviation(paramSequence, paramNumberOfSamples);
        }
        public static IEnumerable<DoubleSample> TopBollinger(this IEnumerable<DoubleSample> paramSequence, int paramNumberOfSamples = 20, double paramNumberOfStandardDeviations = 2.0)
        {
            return manipulator.TopBollinger(paramSequence, paramNumberOfSamples, paramNumberOfStandardDeviations);
        }
        public static IEnumerable<DoubleSample> BottomBollinger(this IEnumerable<DoubleSample> paramSequence, int paramNumberOfSamples = 20, double paramNumberOfStandardDeviations = 2.0)
        {
            return manipulator.BottomBollinger(paramSequence, paramNumberOfSamples, paramNumberOfStandardDeviations);
        }
        public static IEnumerable<DoubleSample> BollingerIndicatorBeforeClipping(this IEnumerable<DoubleSample> paramSequence, double paramFactor)
        {
            return manipulator.BollingerIndicatorBeforeClipping(paramSequence, paramFactor);
        }
        public static IEnumerable<DoubleSample> MACD(this IEnumerable<DoubleSample> paramSequence)
        {
            return manipulator.MACD(paramSequence);
        }
        public static IEnumerable<DoubleSample> MACDHistogram(this IEnumerable<DoubleSample> paramSequence)
        {
            return manipulator.MACDHistogram(paramSequence);
        }
        public static IEnumerable<DoubleSample> SellingBuyingSignal(this IEnumerable<DoubleSample> paramSequence, double paramDecayFactor)
        {
            double returned = 0.0;
            var firstSample = paramSequence.First();
            double lastValue = firstSample.Value;

            yield return new DoubleSample() { Date = firstSample.Date, Value = returned, Valid = false };
            foreach (var sample in paramSequence.Skip(1))
            {
                if ((lastValue < 0) && (sample.Value > -0))
                    returned = 1.0;
                else if (returned > 0)
                    returned = Math.Max(0, returned - paramDecayFactor);

                if ((lastValue > 0) && (sample.Value < -0))
                    returned = -1.0;
                else if (returned < 0)
                    returned = Math.Min(0, returned + paramDecayFactor);

                yield return new DoubleSample() { Date = sample.Date, Valid = sample.Valid, Value = returned };
                lastValue = sample.Value;
            }
        }
        public static IEnumerable<DoubleSample> Clip(this IEnumerable<DoubleSample> paramSequence, double paramClipValue)
        {
            return manipulator.Clip(paramSequence, paramClipValue);
        }
        public static IEnumerable<DoubleSample> Invert(this IEnumerable<DoubleSample> paramSequence)
        {
            return manipulator.Invert(paramSequence);
        }
        public static IEnumerable<DoubleSample> RSI(this IEnumerable<DoubleSample> paramSequence, int paramNumberOfSamples)
        {
            return manipulator.RSI(paramSequence, paramNumberOfSamples);
        }
    }
}
