using System;

namespace Stocks.Entities
{
    public class NumericWrapper<T>
    {
        public T Value { get; set; }
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

        public NumericWrapper(T paramValue,
                              Func<T, T, T> paramAdd,
                              Func<T, T> paramNegate,
                              Func<T, T, T> paramMultiply,
                              Func<T, T, T> paramDivide,
                              Func<T> paramZero,
                              Func<T> paramOne,
                              Func<T, T> paramClone,
                              Func<T, T, bool> paramGreaterThan,
                              Func<T, T, bool> paramGreaterEqualThan,
                              Func<T, T, bool> paramLessThan,
                              Func<T, T, bool> paramLessEqualThan,
                              Func<T, T, bool> paramEqual)
        {
            Value = paramValue;
            add = paramAdd;
            negate = paramNegate;
            multiply = paramMultiply;
            divide = paramDivide;
            zero = paramZero;
            one = paramOne;
            clone = paramClone;
            greaterThan = paramGreaterThan;
            greaterEqualThan = paramGreaterEqualThan;
            lessThan = paramLessThan;
            lessEqualThan = paramLessEqualThan;
            equal = paramEqual;
        }

        public NumericWrapper<T> Zero()
        {
            return Build(zero());
        }
        public NumericWrapper<T> One()
        {
            return Build(one());
        }

        public NumericWrapper<T> Build(T paramValue)
        {
            return new NumericWrapper<T>(paramValue,
                                          add,
                                          negate,
                                          multiply,
                                          divide,
                                          zero,
                                          one,
                                          clone,
                                          greaterThan,
                                          greaterEqualThan,
                                          lessThan,
                                          lessEqualThan,
                                          equal);
        }

        public static NumericWrapper<T> operator +(NumericWrapper<T> p1, NumericWrapper<T> p2)
        {
            return p1.Build(p1.add(p1.Value, p2.Value));
        }

        public static NumericWrapper<T> operator -(NumericWrapper<T> p1, NumericWrapper<T> p2)
        {
            return p1.Build(p1.add(p1.Value, p1.negate(p2.Value)));
        }

        public static NumericWrapper<T> operator -(NumericWrapper<T> p1)
        {
            return p1.Build(p1.negate(p1.Value));
        }

        public static NumericWrapper<T> operator *(NumericWrapper<T> p1, NumericWrapper<T> p2)
        {
            return p1.Build(p1.multiply(p1.Value, p2.Value));
        }

        public static NumericWrapper<T> operator /(NumericWrapper<T> p1, NumericWrapper<T> p2)
        {
            return p1.Build(p1.divide(p1.Value, p2.Value));
        }

        public static bool operator ==(NumericWrapper<T> p1, NumericWrapper<T> p2)
        {
            return p1.equal(p1.Value, p2.Value);
        }
        public override bool Equals(object obj)
        {
            if (obj is T)
                return Value.Equals(obj);
            else if (obj is NumericWrapper<T>)
                return Value.Equals(((NumericWrapper<T>)obj).Value);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static bool operator !=(NumericWrapper<T> p1, NumericWrapper<T> p2)
        {
            return !p1.equal(p1.Value, p2.Value);
        }

        public static bool operator >=(NumericWrapper<T> p1, NumericWrapper<T> p2)
        {
            return p1.greaterEqualThan(p1.Value, p2.Value);
        }

        public static bool operator >(NumericWrapper<T> p1, NumericWrapper<T> p2)
        {
            return p1.greaterThan(p1.Value, p2.Value);
        }

        public static bool operator <=(NumericWrapper<T> p1, NumericWrapper<T> p2)
        {
            return p1.lessEqualThan(p1.Value, p2.Value);
        }

        public static bool operator <(NumericWrapper<T> p1, NumericWrapper<T> p2)
        {
            return p1.lessThan(p1.Value, p2.Value);
        }

        public static NumericWrapper<double> Build(double paramValue)
        {
            return new NumericWrapper<double>(paramValue,
                                              (d1, d2) => d1 + d2,
                                              (d1) => -d1,
                                              (d1, d2) => d1 * d2,
                                              (d1, d2) => d1 / d2,
                                              () => 0.0,
                                              () => 1.0,
                                              (d1) => d1,
                                              (d1, d2) => d1 >= d2,
                                              (d1, d2) => d1 > d2,
                                              (d1, d2) => d1 <= d2,
                                              (d1, d2) => d1 < d2,
                                              (d1, d2) => d1 == d2);
        }
        public double Compare(NumericWrapper<T> p1)
        {
            return this > p1 ? 1.0 : (this == p1 ? 0.0 : -1.0);
        }
    }
}
