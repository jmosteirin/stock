using Stocks.Calculators;
using Stocks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stocks
{
    public static class SequenceHelper
    {
        private static SequenceManipulator<SingleValueSample<double>, SingleValueSampleCalculator<double, SingleValueSample<double>>> manipulator = new SequenceManipulator<SingleValueSample<double>, SingleValueSampleCalculator<double, SingleValueSample<double>>>(new SingleValueSampleCalculator<double, SingleValueSample<double>>());
        public static IEnumerable<SingleValueSample<double>> BufferedTransform(this IEnumerable<SingleValueSample<double>> paramSequence, int paramNumberOfSamples, Func<Queue<SingleValueSample<double>>, SingleValueSample<double>, SingleValueSample<double>> paramTransform)
        {
            return manipulator.BufferedTransform(paramSequence, paramNumberOfSamples, paramTransform);
        }
        public static IEnumerable<SingleValueSample<double>> Transform(this IEnumerable<SingleValueSample<double>> paramSequence, Func<SingleValueSample<double>, SingleValueSample<double>> paramTransform)
        {
            return manipulator.Transform(paramSequence, paramTransform);
        }
        public static IEnumerable<SingleValueSample<double>> SetToZeroSamplesThatNot(this IEnumerable<SingleValueSample<double>> paramSequence, Func<SingleValueSample<double>, bool> paramCondition)
        {
            return manipulator.SetToZeroSamplesThatNot(paramSequence, paramCondition);
        }
        public static IEnumerable<SingleValueSample<double>> OnlyPositives(this IEnumerable<SingleValueSample<double>> paramSequence)
        {
            return manipulator.OnlyPositives(paramSequence);
        }
        public static IEnumerable<SingleValueSample<double>> OnlyNegatives(this IEnumerable<SingleValueSample<double>> paramSequence)
        {
            return manipulator.OnlyNegatives(paramSequence);
        }
        public static IEnumerable<SingleValueSample<double>> Accumulate(this IEnumerable<SingleValueSample<double>> paramSequence, int paramNumberOfSamples)
        {
            return manipulator.Accumulate(paramSequence, paramNumberOfSamples);
        }
        public static IEnumerable<SingleValueSample<double>> Average(this IEnumerable<SingleValueSample<double>> paramSequence, int paramNumberOfSamples)
        {
            return manipulator.Average(paramSequence, paramNumberOfSamples);
        }
        public static IEnumerable<SingleValueSample<double>> ExponentialAverage(this IEnumerable<SingleValueSample<double>> paramSequence, int paramNumberOfSamples)
        {
            return manipulator.ExponentialAverage(paramSequence, paramNumberOfSamples);
        }
        public static IEnumerable<SingleValueSample<double>> Subtract(this IEnumerable<SingleValueSample<double>> paramSequence, IEnumerable<SingleValueSample<double>> paramSecondSequence)
        {
            return manipulator.Subtract(paramSequence, paramSecondSequence);
        }
        public static IEnumerable<SingleValueSample<double>> Add(this IEnumerable<SingleValueSample<double>> paramSequence, IEnumerable<SingleValueSample<double>> paramSecondSequence)
        {
            return manipulator.Add(paramSequence, paramSecondSequence);
        }
        public static IEnumerable<SingleValueSample<double>> Divide(this IEnumerable<SingleValueSample<double>> paramSequence, IEnumerable<SingleValueSample<double>> paramSecondSequence)
        {
            return manipulator.Divide(paramSequence, paramSecondSequence);
        }
        public static IEnumerable<SingleValueSample<double>> ScalarMultiplication(this IEnumerable<SingleValueSample<double>> paramSequence, double paramScalar)
        {
            return manipulator.ScalarMultiplication(paramSequence, paramScalar);
        }
        public static IEnumerable<SingleValueSample<double>> Square(this IEnumerable<SingleValueSample<double>> paramSequence)
        {
            return manipulator.Square(paramSequence);
        }
        public static IEnumerable<SingleValueSample<double>> Sqrt(this IEnumerable<SingleValueSample<double>> paramSequence)
        {
            return manipulator.Sqrt(paramSequence);
        }
        public static IEnumerable<SingleValueSample<double>> Variance(this IEnumerable<SingleValueSample<double>> paramSequence, int paramNumberOfSamples)
        {
            return manipulator.Variance(paramSequence, paramNumberOfSamples);
        }
        public static IEnumerable<SingleValueSample<double>> StandardDeviation(this IEnumerable<SingleValueSample<double>> paramSequence, int paramNumberOfSamples)
        {
            return manipulator.StandardDeviation(paramSequence, paramNumberOfSamples);
        }
        public static IEnumerable<SingleValueSample<double>> TopBollinger(this IEnumerable<SingleValueSample<double>> paramSequence, int paramNumberOfSamples = 20, double paramNumberOfStandardDeviations = 2.0)
        {
            return manipulator.TopBollinger(paramSequence, paramNumberOfSamples, paramNumberOfStandardDeviations);
        }
        public static IEnumerable<SingleValueSample<double>> BottomBollinger(this IEnumerable<SingleValueSample<double>> paramSequence, int paramNumberOfSamples = 20, double paramNumberOfStandardDeviations = 2.0)
        {
            return manipulator.BottomBollinger(paramSequence, paramNumberOfSamples, paramNumberOfStandardDeviations);
        }
        public static IEnumerable<SingleValueSample<double>> BollingerIndicatorBeforeClipping(this IEnumerable<SingleValueSample<double>> paramSequence, double paramFactor)
        {
            return manipulator.BollingerIndicatorBeforeClipping(paramSequence, paramFactor);
        }
        public static IEnumerable<SingleValueSample<double>> MACD(this IEnumerable<SingleValueSample<double>> paramSequence)
        {
            return manipulator.MACD(paramSequence);
        }
        public static IEnumerable<SingleValueSample<double>> MACDHistogram(this IEnumerable<SingleValueSample<double>> paramSequence)
        {
            return manipulator.MACDHistogram(paramSequence);
        }
        public static IEnumerable<SingleValueSample<double>> SellingBuyingSignal(this IEnumerable<SingleValueSample<double>> paramSequence, double paramDecayFactor)
        {
            double returned = 0.0;
            var firstSample = paramSequence.First();
            double lastValue = firstSample.Value;

            yield return new SingleValueSample<double>() { Date = firstSample.Date, Value = returned, Valid = false };
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

                yield return new SingleValueSample<double>() { Date = sample.Date, Valid = sample.Valid, Value = returned };
                lastValue = sample.Value;
            }
        }
        public static IEnumerable<SingleValueSample<double>> Clip(this IEnumerable<SingleValueSample<double>> paramSequence, double paramClipValue)
        {
            return manipulator.Clip(paramSequence, paramClipValue);
        }
        public static IEnumerable<SingleValueSample<double>> Invert(this IEnumerable<SingleValueSample<double>> paramSequence)
        {
            return manipulator.Invert(paramSequence);
        }
        public static IEnumerable<SingleValueSample<double>> RSI(this IEnumerable<SingleValueSample<double>> paramSequence, int paramNumberOfSamples)
        {
            return manipulator.RSI(paramSequence, paramNumberOfSamples);
        }
    }
}
