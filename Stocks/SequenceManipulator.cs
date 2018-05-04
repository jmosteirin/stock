using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks
{
    public class SequenceManipulator<TSample, TSampleCalculator>
        where TSample : Sample
        where TSampleCalculator : SampleCalculator<TSample>
    {
        private TSampleCalculator calculator = null;

        public SequenceManipulator(TSampleCalculator paramCalculator)
        {
            calculator = paramCalculator ?? throw new ArgumentNullException(@"paramCalculator");
        }

        public IEnumerable<TSample> BufferedTransform(IEnumerable<TSample> paramSequence, int paramNumberOfSamples, Func<Queue<TSample>, TSample, TSample> paramTransform)
        {
            Queue<TSample> buffer = new Queue<TSample>();

            foreach (var sample in paramSequence)
            {
                buffer.Enqueue(sample);

                if (buffer.Count() > paramNumberOfSamples)
                    buffer.Dequeue();

                if (buffer.Any(s => !s.IsValid()))
                    yield return calculator.BuildInvalid(sample);
                else
                {
                    yield return paramTransform(buffer, sample);
                }
            }
        }
        public IEnumerable<TSample> Transform(IEnumerable<TSample> paramSequence, Func<TSample, TSample> paramTransform)
        {
            foreach (var sample in paramSequence)
            {
                if (!sample.IsValid())
                    yield return calculator.BuildInvalid(sample);
                else
                    yield return paramTransform(sample);
            }
        }
        public IEnumerable<TSample> SetToZeroSamplesThatNot(IEnumerable<TSample> paramSequence, Func<TSample, bool> paramCondition)
        {
            return Transform(paramSequence, s =>
            {
                if (!s.IsValid())
                    return calculator.BuildInvalid(s);

                return paramCondition(s) ? calculator.Clone(s) : calculator.Clone(calculator.GetZero(s));

            });

        }
        public IEnumerable<TSample> OnlyPositives(IEnumerable<TSample> paramSequence)
        {
            return SetToZeroSamplesThatNot(paramSequence, s => (calculator.Compare(s, calculator.GetZero(s)) > 0));
        }
        public IEnumerable<TSample> OnlyNegatives(IEnumerable<TSample> paramSequence)
        {
            return SetToZeroSamplesThatNot(paramSequence, s => (calculator.Compare(s, calculator.GetZero(s)) < 0));
        }
        public IEnumerable<TSample> Accumulate(IEnumerable<TSample> paramSequence, int paramNumberOfSamples)
        {
            return BufferedTransform(paramSequence, paramNumberOfSamples, (buffer, sample) => calculator.Add(sample, buffer.ToArray()));
        }
        public IEnumerable<TSample> Average(IEnumerable<TSample> paramSequence, int paramNumberOfSamples)
        {
            return BufferedTransform(paramSequence, paramNumberOfSamples, (buffer, sample) => calculator.Average(sample, buffer.ToArray()));
        }
        public IEnumerable<TSample> ExponentialAverage(IEnumerable<TSample> paramSequence, int paramNumberOfSamples)
        {
            var multiplier = 2.0 / (paramNumberOfSamples + 1.0);
            var count = 0;

            TSample previousSample = null;
            foreach (var sample in paramSequence)
            {
                if (count <= paramNumberOfSamples)
                    yield return previousSample = sample;
                else
                    yield return previousSample = calculator.Add(calculator.ScalarMultiply(calculator.Subtract(sample, previousSample), multiplier), previousSample);
                count++;
            }
        }
        public IEnumerable<TSample> Subtract(IEnumerable<TSample> paramSequence, IEnumerable<TSample> paramSecondSequence)
        {
            var enumerator1 = paramSequence.GetEnumerator();
            var enumerator2 = paramSecondSequence.GetEnumerator();
            while ((enumerator1.MoveNext()) && (enumerator2.MoveNext()))
                yield return calculator.Subtract(enumerator1.Current, enumerator2.Current);
        }
        public IEnumerable<TSample> Add(IEnumerable<TSample> paramSequence, IEnumerable<TSample> paramSecondSequence)
        {
            var enumerator1 = paramSequence.GetEnumerator();
            var enumerator2 = paramSecondSequence.GetEnumerator();
            while ((enumerator1.MoveNext()) && (enumerator2.MoveNext()))
                yield return calculator.Add(enumerator1.Current, enumerator2.Current);
        }
        public IEnumerable<TSample> Divide(IEnumerable<TSample> paramSequence, IEnumerable<TSample> paramSecondSequence)
        {
            var enumerator1 = paramSequence.GetEnumerator();
            var enumerator2 = paramSecondSequence.GetEnumerator();
            while ((enumerator1.MoveNext()) && (enumerator2.MoveNext()))
                yield return calculator.Divide(enumerator1.Current, enumerator2.Current);
        }
        public IEnumerable<TSample> ScalarMultiplication(IEnumerable<TSample> paramSequence, double paramScalar)
        {
            foreach (var sample in paramSequence)
                yield return calculator.ScalarMultiply(sample, paramScalar);
        }
        public IEnumerable<TSample> Square(IEnumerable<TSample> paramSequence)
        {
            foreach (var sample in paramSequence)
                yield return calculator.Pow(sample, 2.0);
        }
        public IEnumerable<TSample> Sqrt(IEnumerable<TSample> paramSequence)
        {
            foreach (var sample in paramSequence)
                yield return calculator.Pow(sample, 0.5);
        }
        public IEnumerable<TSample> Variance(IEnumerable<TSample> paramSequence, int paramNumberOfSamples)
        {
            return Average(Square(Subtract(paramSequence, Average(paramSequence, paramNumberOfSamples))), paramNumberOfSamples);
        }
        public IEnumerable<TSample> StandardDeviation(IEnumerable<TSample> paramSequence, int paramNumberOfSamples)
        {
            return Sqrt(Variance(paramSequence, paramNumberOfSamples));
        }
        public IEnumerable<TSample> TopBollinger(IEnumerable<TSample> paramSequence, int paramNumberOfSamples = 20, double paramNumberOfStandardDeviations = 2.0)
        {
            return Add(Average(paramSequence, paramNumberOfSamples), ScalarMultiplication(StandardDeviation(paramSequence, paramNumberOfSamples), paramNumberOfStandardDeviations));
        }
        public IEnumerable<TSample> BottomBollinger(IEnumerable<TSample> paramSequence, int paramNumberOfSamples = 20, double paramNumberOfStandardDeviations = 2.0)
        {
            return Subtract(Average(paramSequence, paramNumberOfSamples), ScalarMultiplication(StandardDeviation(paramSequence, paramNumberOfSamples), paramNumberOfStandardDeviations));
        }
        public IEnumerable<TSample> BollingerIndicatorBeforeClipping(IEnumerable<TSample> paramSequence, double paramFactor)
        {
            var bollingerTop = TopBollinger(paramSequence).GetEnumerator();
            var bollingerBottom = BottomBollinger(paramSequence).GetEnumerator();
            var average = Average(paramSequence, 20).GetEnumerator();
            var samples = paramSequence.GetEnumerator();
            while (samples.MoveNext() && bollingerTop.MoveNext() && bollingerBottom.MoveNext() && average.MoveNext())
            {
                var comparison = calculator.Compare(samples.Current, average.Current);
                var subtract = calculator.Subtract(samples.Current, average.Current);
                var value = comparison == 0 ? calculator.GetZero(samples.Current) :
                            ((comparison > 0) ?
                            calculator.Divide(subtract, calculator.ScalarMultiply(calculator.Subtract(bollingerTop.Current, average.Current), paramFactor)) :
                            calculator.Negate(calculator.Divide(subtract, calculator.ScalarMultiply(calculator.Subtract(bollingerBottom.Current, average.Current), paramFactor))));
                yield return value;
            }

        }
        public IEnumerable<TSample> MACD(IEnumerable<TSample> paramSequence)
        {
            return Subtract(ExponentialAverage(paramSequence, 12), ExponentialAverage(paramSequence, 26));
        }
        public IEnumerable<TSample> MACDHistogram(IEnumerable<TSample> paramSequence)
        {
            return Subtract(MACD(paramSequence), ExponentialAverage(paramSequence, 9));
        }
        public IEnumerable<TSample> Clip(IEnumerable<TSample> paramSequence, double paramClipValue)
        {
            if (paramClipValue <= 0)
                throw new ArgumentException(@"paramClipValue");


            foreach (var sample in paramSequence)
            {
                var positiveClipSample = calculator.GetValue(sample, paramClipValue);
                var negativeClipSample = calculator.GetValue(sample, paramClipValue);
                if (calculator.Compare(sample, positiveClipSample) > 0)
                    yield return positiveClipSample;
                else if (calculator.Compare(sample, negativeClipSample) < 0)
                    yield return negativeClipSample;
                yield return calculator.Clone(sample);
            }
        }
        public IEnumerable<TSample> Invert(IEnumerable<TSample> paramSequence)
        {
            foreach (var sample in paramSequence)
                yield return calculator.Negate(sample);
        }
        public IEnumerable<TSample> RSI(IEnumerable<TSample> paramSequence, int paramNumberOfSamples)
        {
            var collapseToOne = Transform(paramSequence, s =>
            {
                var zero = calculator.GetZero(s);
                var comparison = calculator.Compare(s, zero);
                if (comparison > 0)
                    return calculator.GetUnit(s);
                else if (comparison < 0)
                    return calculator.Negate(calculator.GetUnit(s));
                else
                    return calculator.GetZero(s);
            });
            return Transform(Divide(Accumulate(OnlyPositives(collapseToOne), paramNumberOfSamples),
                    Accumulate(Invert(OnlyNegatives(collapseToOne)), paramNumberOfSamples)),
                    s =>
                    {
                        var sample100 = calculator.GetValue(s, 100);
                        var sample1 = calculator.GetUnit(s);
                        return calculator.Subtract(sample100, calculator.Divide(sample100, calculator.Add(sample1, s)));
                    });
        }
    }
}
