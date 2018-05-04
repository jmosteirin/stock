using System;

namespace Stocks.Entities
{
    public class SingleValueSample<T> : Sample
    {
        public SingleValueSample()
        {
        }
        public SingleValueSample(NumericWrapper<T> paramValue, DateTime paramDate)
        {
            InternalValue = paramValue;
            Date = paramDate;
            Valid = true;
            Infinite = false;
        }
        public T Value { get => InternalValue.Value; set => InternalValue.Value = value; }
        internal NumericWrapper<T> InternalValue { get; set; }
        public DateTime Date { get; set; }
        public bool Valid { get; set; }
        public bool Infinite { get; set; }
        public override bool IsInfinite()
        {
            return Infinite;
        }
        public override bool IsValid()
        {
            return Valid;
        }
    }
}
