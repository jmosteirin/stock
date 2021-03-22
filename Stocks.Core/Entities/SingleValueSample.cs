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
            Build(paramValue, paramDate);
        }
        private void Build(NumericWrapper<T> paramValue, DateTime paramDate)
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
        public static SingleValueSample<T> operator +(SingleValueSample<T> p1, SingleValueSample<T> p2)
        {
            return p1.Build(p1.add(p1.Value, p2.Value));
        }

        public static SingleValueSample<T> operator -(SingleValueSample<T> p1, SingleValueSample<T> p2)
        {
            return p1.Build(p1.add(p1.Value, p1.negate(p2.Value)));
        }

        public static SingleValueSample<T> operator -(SingleValueSample<T> p1)
        {
            return p1.Build(p1.negate(p1.Value));
        }

        public static SingleValueSample<T> operator *(SingleValueSample<T> p1, SingleValueSample<T> p2)
        {
            if (!p1.Infinite && p1.Valid && !p2.Infinite && p2.Valid)
                return new SingleValueSample<T>(p1.InternalValue * p2.InternalValue, p1.Date);
            else if (!p1.Valid || !p2.Valid)
                return new SingleValueSample<T>() { Valid = false, Date = p1.Date };
            else
            {
                if (p1.Infinite && p2.Infinite)
                    return new SingleValueSample<T>(p1.InternalValue * p2.InternalValue, p1.Date) { Infinite = true };

                if (p1.Infinite || p2.Infinite)
                {
                    NumericWrapper<T> zero = p1.InternalValue.Zero();
                    if ((p1.InternalValue == zero) || (p2.InternalValue == zero))
                        return new SingleValueSample<T>() { Valid = false, Date = p1.Date };
                    else
                        return new SingleValueSample<T>(p1.InternalValue * p2.InternalValue, p1.Date) { Infinite = true };
                }
                else
                    return null;//shouldnt ever happen
            }
        }

        public static SingleValueSample<T> operator /(SingleValueSample<T> p1, SingleValueSample<T> p2)
        {
            return p1.Build(p1.divide(p1.Value, p2.Value));
        }
    }
}
