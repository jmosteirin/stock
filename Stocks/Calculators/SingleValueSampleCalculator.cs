using Stocks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Calculators
{
    public class SingleValueSampleCalculator<T> : SampleCalculator<SingleValueSample<T>>
    {
        private Func<T, T, T> add = null;
        private Func<T, T> negate = null;
        private Func<T, T, T> multiply = null;
        private Func<T, T, T> divide = null;
        private Func<T> zero = null;
        private Func<T> one = null;
        private Func<T, T> clone = null;
        private Func<T, T, bool> greaterThan = null;
        private Func<T, T, bool> greaterEqualThan = null;
        private Func<T, T, bool> lessThan = null;
        private Func<T, T, bool> lessEqualThan = null;
        private Func<T, T, bool> equal = null;
        public SingleValueSampleCalculator()
        {

        }
        public override SingleValueSample<T> Add(SingleValueSample<T> paramSample, params SingleValueSample<T>[] paramSamples)
        {
            if (paramSample.Valid || paramSamples.Any(s => !s.Valid))
                return BuildInvalid(paramSample);

            var zero = paramSample.Value.Zero();

            var positiveInfinites = (paramSample.Infinite && paramSample.Value > zero) || paramSamples.Any(s => s.Infinite && s.Value > zero);
            var negativeInfinites = (paramSample.Infinite && paramSample.Value < zero) || paramSamples.Any(s => s.Infinite && s.Value < zero);

            if (positiveInfinites && negativeInfinites)
                return BuildInvalid(paramSample);
            else if (positiveInfinites)
                return BuildPositiveInfinite(paramSample);
            else if (negativeInfinites)
                return BuildNegativeInfinite(paramSample);
            else
            {
                var returned = Clone(paramSample);
                returned.Value += paramSamples.Select(s => s.Value).Aggregate((v1, v2) => v1 + v2);
                return returned;
            }
        }

        private SingleValueSample<T> BuildPositiveInfinite(SingleValueSample<T> paramSample)
        {
            var one = paramSample.Value.One();

            return new SingleValueSample<T>() { Value = one, Date = paramSample.Date, Infinite = true, Valid = true };
        }

        private SingleValueSample<T> BuildNegativeInfinite(SingleValueSample<T> paramSample)
        {
            return new SingleValueSample<T>() { Value = -(paramSample.Value.One()), Date = paramSample.Date, Infinite = true, Valid = true };
        }

        public override SingleValueSample<T> BuildInvalid(SingleValueSample<T> paramSample)
        {
            return new SingleValueSample<T>() { Date = paramSample.Date, Valid = false };
        }

        public override SingleValueSample<T> Clone(SingleValueSample<T> paramSample)
        {
            return new SingleValueSample<T>() { Date = paramSample.Date, Value = paramSample.Value, Infinite = paramSample.Infinite, Valid = paramSample.Valid };
        }

        public override double Compare(SingleValueSample<T> paramFirstSample, SingleValueSample<T> paramSecondSample)
        {
            if (!(paramFirstSample.Valid && paramSecondSample.Valid))
                throw new Exception(@"Operation on a non valid sample");

            if (paramFirstSample.Infinite && paramSecondSample.Infinite)
            {
                if (paramFirstSample.Value == paramSecondSample.Value)
                    throw new Exception(@"Operation on a non valid sample");
                else
                    return paramFirstSample.Value.Compare(paramSecondSample.Value);
            }

            if (paramFirstSample.Infinite)
                return paramFirstSample.Value.Compare(paramFirstSample.Value.Zero());

            if (paramSecondSample.Infinite)
                return paramFirstSample.Value.Zero().Compare(paramFirstSample.Value);

            return paramFirstSample.Value.Compare(paramSecondSample.Value);
        }

        public override SingleValueSample<T> Divide(SingleValueSample<T> paramFirstSample, SingleValueSample<T> paramSecondSample)
        {
            throw new NotImplementedException();
        }

        public override SingleValueSample<T> GetUnit(SingleValueSample<T> paramSample)
        {
            throw new NotImplementedException();
        }

        public override SingleValueSample<T> GetValue(SingleValueSample<T> paramSample, double paramClipValue)
        {
            throw new NotImplementedException();
        }

        public override SingleValueSample<T> GetZero(SingleValueSample<T> paramSample)
        {
            throw new NotImplementedException();
        }

        public override SingleValueSample<T> Pow(SingleValueSample<T> paramSample, double paramScalar)
        {
            throw new NotImplementedException();
        }

        public override SingleValueSample<T> ScalarMultiply(SingleValueSample<T> paramSample, double paramScalar)
        {
            throw new NotImplementedException();
        }

        public override SingleValueSample<T> Subtract(SingleValueSample<T> paramFirstSample, SingleValueSample<T> paramSecondSample)
        {
            throw new NotImplementedException();
        }
    }
}
