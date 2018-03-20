using Stocks.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stocks
{
    public static class SequenceHelper
    {
        public static IEnumerable<Sample> BufferedTransform(this IEnumerable<Sample> paramSequence, int paramNumberOfSamples, Func<Queue<Sample>, Sample, Sample> paramTransform)
        {
            Queue<Sample> buffer = new Queue<Sample>();

            foreach (var sample in paramSequence)
            {
                buffer.Enqueue(sample);

                if (buffer.Count() > paramNumberOfSamples)
                    buffer.Dequeue();

                if (buffer.Any(s => !s.Valid))
                    yield return new Sample() { Value = 0, Date = sample.Date, Valid = false };
                else
                {
                    yield return paramTransform(buffer, sample);
                }
            }
        }
        public static IEnumerable<Sample> Transform(this IEnumerable<Sample> paramSequence, Func<Sample, Sample> paramTransform)
        {
            foreach (var sample in paramSequence)
            {
                if (!sample.Valid)
                    yield return new Sample() { Valid = false, Date = sample.Date };
                else
                    yield return paramTransform(sample);
            }
        }
        public static IEnumerable<Sample> SetToZeroSamplesThatNot(this IEnumerable<Sample> paramSequence, Func<Sample, bool> paramCondition)
        {
            return paramSequence.Transform(s => paramCondition(s) ? s.Clone() : new Sample() { Value = 0, Date = s.Date, Infinite = s.Infinite, Valid = s.Valid });
        }
        public static IEnumerable<Sample> OnlyPositives(this IEnumerable<Sample> paramSequence)
        {
            return paramSequence.SetToZeroSamplesThatNot(s => (s.Value > 0));
        }
        public static IEnumerable<Sample> OnlyNegatives(this IEnumerable<Sample> paramSequence)
        {
            return paramSequence.SetToZeroSamplesThatNot(s => (s.Value < 0));
        }
        public static IEnumerable<Sample> Accumulate(this IEnumerable<Sample> paramSequence, int paramNumberOfSamples)
        {
            return paramSequence.BufferedTransform(paramNumberOfSamples, (buffer, sample) =>
            {
                var infinites = buffer.Where(s => s.Infinite).Select(s => s.Value).Distinct().ToArray();
                if (infinites.Length == 0)
                    return new Sample() { Value = buffer.Select(v => v.Value).Sum(), Date = sample.Date, Valid = buffer.Count() == paramNumberOfSamples };
                else if (infinites.Length == 1)
                    return new Sample() { Value = infinites[0], Date = sample.Date, Valid = buffer.Count() == paramNumberOfSamples, Infinite = true };
                else
                    return new Sample() { Value = 0, Date = sample.Date, Valid = false };
            });
        }
        public static IEnumerable<Sample> Average(this IEnumerable<Sample> paramSequence, int paramNumberOfSamples)
        {
            return paramSequence.BufferedTransform(paramNumberOfSamples, (buffer, sample) =>
            {
                var infinites = buffer.Where(s => s.Infinite).Select(s => s.Value).Distinct().ToArray();
                if (infinites.Length == 0)
                    return new Sample() { Value = buffer.Select(v => v.Value).Average(), Date = sample.Date, Valid = buffer.Count() == paramNumberOfSamples };
                else if (infinites.Length == 1)
                    return new Sample() { Value = infinites[0], Date = sample.Date, Valid = buffer.Count() == paramNumberOfSamples, Infinite = true };
                else
                    return new Sample() { Value = 0, Date = sample.Date, Valid = false };
            });
        }
        public static IEnumerable<Sample> ExponentialAverage(this IEnumerable<Sample> paramSequence, int paramNumberOfSamples)
        {
            var multiplier = 2.0/(paramNumberOfSamples + 1.0);
            var count = 0;

            Sample previousSample = null;
            foreach (var sample in paramSequence)
            {
                if (count <= paramNumberOfSamples)
                    yield return previousSample = sample;
                else
                    yield return previousSample = sample.Subtract(previousSample).Multiply(multiplier).Add(previousSample);
                count++;
            }
        }
        public static IEnumerable<Sample> Subtract(this IEnumerable<Sample> paramSequence, IEnumerable<Sample> paramSecondSequence)
        {
            var enumerator1 = paramSequence.GetEnumerator();
            var enumerator2 = paramSecondSequence.GetEnumerator();
            while ((enumerator1.MoveNext()) && (enumerator2.MoveNext()))
                yield return enumerator1.Current.Subtract(enumerator2.Current);
        }
        public static IEnumerable<Sample> Add(this IEnumerable<Sample> paramSequence, IEnumerable<Sample> paramSecondSequence)
        {
            var enumerator1 = paramSequence.GetEnumerator();
            var enumerator2 = paramSecondSequence.GetEnumerator();
            while ((enumerator1.MoveNext()) && (enumerator2.MoveNext()))
                yield return enumerator1.Current.Add(enumerator2.Current);
        }
        public static IEnumerable<Sample> Divide(this IEnumerable<Sample> paramSequence, IEnumerable<Sample> paramSecondSequence)
        {
            var enumerator1 = paramSequence.GetEnumerator();
            var enumerator2 = paramSecondSequence.GetEnumerator();
            while ((enumerator1.MoveNext()) && (enumerator2.MoveNext()))
                yield return enumerator1.Current.Divide(enumerator2.Current);
        }
        public static IEnumerable<Sample> ScalarMultiplication(this IEnumerable<Sample> paramSequence, double paramScalar)
        {
            foreach (var sample in paramSequence)
                yield return sample.Multiply(paramScalar);
        }
        public static IEnumerable<Sample> Square(this IEnumerable<Sample> paramSequence)
        {
            foreach (var sample in paramSequence)
                yield return sample.Square();
        }
        public static IEnumerable<Sample> Sqrt(this IEnumerable<Sample> paramSequence)
        {
            foreach (var sample in paramSequence)
                yield return sample.SquareRoot();
        }
        public static IEnumerable<Sample> Variance(this IEnumerable<Sample> paramSequence, int paramNumberOfSamples)
        {
            return paramSequence.Subtract(paramSequence.Average(paramNumberOfSamples)).Square().Average(paramNumberOfSamples);
        }
        public static IEnumerable<Sample> StandardDeviation(this IEnumerable<Sample> paramSequence, int paramNumberOfSamples)
        {
            return paramSequence.Variance(paramNumberOfSamples).Sqrt();
        }
        public static IEnumerable<Sample> TopBollinger(this IEnumerable<Sample> paramSequence, int paramNumberOfSamples = 20, double paramNumberOfStandardDeviations = 2.0)
        {
            return paramSequence.Average(paramNumberOfSamples).
                Add(paramSequence.StandardDeviation(paramNumberOfSamples).
                ScalarMultiplication(paramNumberOfStandardDeviations));
        }
        public static IEnumerable<Sample> BottomBollinger(this IEnumerable<Sample> paramSequence, int paramNumberOfSamples = 20, double paramNumberOfStandardDeviations = 2.0)
        {
            return paramSequence.Average(paramNumberOfSamples).
                Subtract(paramSequence.StandardDeviation(paramNumberOfSamples).
                ScalarMultiplication(paramNumberOfStandardDeviations));
        }
        public static IEnumerable<Sample> BollingerIndicatorBeforeClipping(this IEnumerable<Sample> paramSequence, double paramFactor)
        {
            var bollingerTop = paramSequence.TopBollinger().GetEnumerator();
            var bollingerBottom = paramSequence.BottomBollinger().GetEnumerator();
            var average = paramSequence.Average(20).GetEnumerator();
            var samples = paramSequence.GetEnumerator();
            while (samples.MoveNext() && bollingerTop.MoveNext() && bollingerBottom.MoveNext() && average.MoveNext())
            {
                var value = (samples.Current.Value == average.Current.Value) ? 0 :
                            ((samples.Current.Value > average.Current.Value) ?
                            (samples.Current.Value - average.Current.Value) / (paramFactor * (bollingerTop.Current.Value - average.Current.Value)) :
                            -(samples.Current.Value - average.Current.Value) / (paramFactor * (bollingerBottom.Current.Value - average.Current.Value)));

                yield return new Sample()
                {
                    Valid = samples.Current.Valid && bollingerTop.Current.Valid && bollingerBottom.Current.Valid,
                    Date = samples.Current.Date,
                    Value = value
                };
            }

        }
        public static IEnumerable<Sample> MACD(this IEnumerable<Sample> paramSequence)
        {
            return paramSequence.ExponentialAverage(12).Subtract(paramSequence.ExponentialAverage(26));
        }
        public static IEnumerable<Sample> MACDHistogram(this IEnumerable<Sample> paramSequence)
        {
            return paramSequence.MACD().Subtract(paramSequence.ExponentialAverage(9));
        }
        public static IEnumerable<Sample> SellingBuyingSignal(this IEnumerable<Sample> paramSequence, double paramDecayFactor)
        {
            double returned = 0.0;
            var firstSample = paramSequence.First();
            double lastValue = firstSample.Value;

            yield return new Sample() { Date = firstSample.Date, Value = returned, Valid = false };
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

                yield return new Sample() { Date = sample.Date, Valid = sample.Valid, Value = returned };
                lastValue = sample.Value;
            }
        }
        public static IEnumerable<Sample> Clip(this IEnumerable<Sample> paramSequence, double paramClipValue)
        {
            if (paramClipValue <= 0)
                throw new ArgumentException(@"paramClipValue");
            foreach (var sample in paramSequence)
            {
                var value = sample.Value;
                if (value > paramClipValue)
                    value = paramClipValue;
                else if (value < -paramClipValue)
                    value = -paramClipValue;
                yield return new Sample() { Value = value, Date = sample.Date, Valid = sample.Valid };
            }
        }
        public static IEnumerable<Sample> Invert(this IEnumerable<Sample> paramSequence)
        {
            foreach (var sample in paramSequence)
                yield return new Sample() { Value = -sample.Value, Date = sample.Date, Valid = sample.Valid };
        }
        public static IEnumerable<Sample> RSI(this IEnumerable<Sample> paramSequence, int paramNumberOfSamples)
        {
            var collapseToOne = paramSequence.Transform(s =>
            {
                if (s.Value > 0)
                    return new Sample(1.0, s.Date);
                else if(s.Value < 0)
                    return new Sample(-1.0, s.Date);
                else
                    return new Sample(0.0, s.Date);
            });
            return collapseToOne.OnlyPositives().Accumulate(paramNumberOfSamples).
                    Divide(collapseToOne.OnlyNegatives().Invert().Accumulate(paramNumberOfSamples)).
                    Transform(s => {
                        var sample100 = new Sample(100, s.Date);
                        var sample1 = new Sample(1, s.Date);
                        return sample100.Subtract(sample100.Divide(sample1.Add(s)));
                    });
        }
    }
}
