using Stocks.Entities;
using System;
using System.Linq;

namespace Stocks.Calculators
{
    public class SingleValueSampleCalculator<T, S> : SampleCalculator<S>
        where S : SingleValueSample<T>, new()
    {
        public SingleValueSampleCalculator()
        {

        }
        public override S Add(S paramSample, params S[] paramSamples)
        {
            if (paramSample.Valid || paramSamples.Any(s => !s.Valid))
                return BuildInvalid(paramSample);

            var zero = paramSample.InternalValue.Zero();

            var positiveInfinites = (paramSample.Infinite && paramSample.InternalValue > zero) || paramSamples.Any(s => s.Infinite && s.InternalValue > zero);
            var negativeInfinites = (paramSample.Infinite && paramSample.InternalValue < zero) || paramSamples.Any(s => s.Infinite && s.InternalValue < zero);

            if (positiveInfinites && negativeInfinites)
                return BuildInvalid(paramSample);
            else if (positiveInfinites)
                return BuildPositiveInfinite(paramSample);
            else if (negativeInfinites)
                return BuildNegativeInfinite(paramSample);
            else
            {
                var returned = Clone(paramSample);
                returned.InternalValue += paramSamples.Select(s => s.InternalValue).Aggregate((v1, v2) => v1 + v2);
                return returned;
            }
        }

        private S BuildPositiveInfinite(S paramSample)
        {
            var one = paramSample.InternalValue.One();

            return new S() { InternalValue = one, Date = paramSample.Date, Infinite = true, Valid = true };
        }

        private S BuildNegativeInfinite(S paramSample)
        {
            return new S() { InternalValue = -(paramSample.InternalValue.One()), Date = paramSample.Date, Infinite = true, Valid = true };
        }

        public override S BuildInvalid(S paramSample)
        {
            return new S() { Date = paramSample.Date, Valid = false };
        }

        public override S Clone(S paramSample)
        {
            return new S() { Date = paramSample.Date, InternalValue = paramSample.InternalValue, Infinite = paramSample.Infinite, Valid = paramSample.Valid };
        }

        public override double Compare(S paramFirstSample, S paramSecondSample)
        {
            if (!(paramFirstSample.Valid && paramSecondSample.Valid))
                throw new Exception(@"Operation on a non valid sample");

            if (paramFirstSample.Infinite && paramSecondSample.Infinite)
            {
                if (paramFirstSample.InternalValue == paramSecondSample.InternalValue)
                    throw new Exception(@"Operation on a non valid sample");
                else
                    return paramFirstSample.InternalValue.Compare(paramSecondSample.InternalValue);
            }

            if (paramFirstSample.Infinite)
                return paramFirstSample.InternalValue.Compare(paramFirstSample.InternalValue.Zero());

            if (paramSecondSample.Infinite)
                return paramFirstSample.InternalValue.Zero().Compare(paramFirstSample.InternalValue);

            return paramFirstSample.InternalValue.Compare(paramSecondSample.InternalValue);
        }

        public override S Divide(S paramFirstSample, S paramSecondSample)
        {
            throw new NotImplementedException();
        }

        public override S GetUnit(S paramSample)
        {
            throw new NotImplementedException();
        }

        public override S GetValue(S paramSample, double paramClipValue)
        {
            throw new NotImplementedException();
        }

        public override S GetZero(S paramSample)
        {
            throw new NotImplementedException();
        }

        public override S Pow(S paramSample, double paramScalar)
        {
            throw new NotImplementedException();
        }

        public override S ScalarMultiply(S paramSample, double paramScalar)
        {
            throw new NotImplementedException();
        }

        public override S Subtract(S paramFirstSample, S paramSecondSample)
        {
            throw new NotImplementedException();
        }
    }
}
