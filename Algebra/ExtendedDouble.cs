using System;
using BinaryOperator = System.Func<Algebra.ExtendedDouble, Algebra.ExtendedDouble, Algebra.ExtendedDouble>;

namespace Algebra
{
    public class ExtendedDouble
    {
        private readonly double? _value;
        private readonly bool valid = false;
        private readonly bool infinite = false;

        public bool Valid => valid;

        private ExtendedDouble(double? paramValue, bool paramInfinite, bool paramValid)
        {
            if(!paramValid && (paramValue != null))
                throw new ArgumentException(@"Invalid doubles must have null value");

            if (paramValid && (paramValue == null))
                throw new ArgumentException(@"Valid doubles must have a value");

            if (paramInfinite && (paramValue != -1) && (paramValue != 1))
                throw new ArgumentException(@"Infinite value must be either -1 or 1");

            valid = paramValid;
            infinite = paramInfinite;
            _value = paramValue;
        }

        public ExtendedDouble(double paramValue)
        {
            _value = paramValue;
            valid = true;
            infinite = false;
        }

        public static ExtendedDouble BuildInvalid()
        {
            return new ExtendedDouble(null, false, false);
        }

        public static ExtendedDouble PositiveInfinite()
        {
            return new ExtendedDouble(1, true, true);
        }

        public static ExtendedDouble NegativeInfinite()
        {
            return new ExtendedDouble(-1, true, true);
        }

        public static ExtendedDouble ExecuteOperation(
            ExtendedDouble p1, 
            ExtendedDouble p2,
            BinaryOperator paramNoneInfinite,
            BinaryOperator paramFirstInfiniteSecondZero,
            BinaryOperator paramFirstInfiniteSecondNonZero,
            BinaryOperator paramFirstZeroSecondInfinite,
            BinaryOperator paramFirstNonZeroSecondInfinite,
            BinaryOperator paramBothInfinite)
        {
            if (!(p1.Valid && p2.Valid)) return BuildInvalid();
            if (!(p1.infinite || p2.infinite)) return paramNoneInfinite(p1, p2);
            if (p1.infinite && p2.infinite) return paramBothInfinite(p1, p2);
            var p2Zero = (!p2.infinite) && (p2._value == 0);
            if (p1.infinite && p2Zero) return paramFirstInfiniteSecondZero(p1, p2);
            if (p1.infinite && !p2Zero) return paramFirstInfiniteSecondNonZero(p1, p2);
            var p1Zero = (!p1.infinite) && (p1._value == 0);
            if (p2.infinite && p1Zero) return paramFirstZeroSecondInfinite(p1, p2);
            if (p2.infinite && !p1Zero) return paramFirstNonZeroSecondInfinite(p1, p2);
            return BuildInvalid();
        }

        public static ExtendedDouble operator +(ExtendedDouble p1, ExtendedDouble p2)
        {
            return ExecuteOperation(p1, p2,
             paramNoneInfinite: (_p1, _p2) => new ExtendedDouble(_p1._value + _p2._value, false, true),
             paramFirstInfiniteSecondZero: (_p1, _p2) => new ExtendedDouble(_p1._value, true, true),
             paramFirstInfiniteSecondNonZero: (_p1, _p2) => new ExtendedDouble(_p1._value, true, true),
             paramFirstZeroSecondInfinite: (_p1, _p2) => new ExtendedDouble(_p2._value, true, true),
             paramFirstNonZeroSecondInfinite: (_p1, _p2) => new ExtendedDouble(_p2._value, true, true),
             paramBothInfinite: (_p1, _p2) => (_p1._value != _p2._value) ? BuildInvalid() : new ExtendedDouble(_p1._value, true, true));
        }

        public static ExtendedDouble operator -(ExtendedDouble p1, ExtendedDouble p2)
        {
            return ExecuteOperation(p1, p2,
             paramNoneInfinite: (_p1, _p2) => new ExtendedDouble(_p1._value - _p2._value, false, true),
             paramFirstInfiniteSecondZero: (_p1, _p2) => new ExtendedDouble(_p1._value, true, true),
             paramFirstInfiniteSecondNonZero: (_p1, _p2) => new ExtendedDouble(_p1._value, true, true),
             paramFirstZeroSecondInfinite: (_p1, _p2) => new ExtendedDouble(-_p2._value, true, true),
             paramFirstNonZeroSecondInfinite: (_p1, _p2) => new ExtendedDouble(-_p2._value, true, true),
             paramBothInfinite: (_p1, _p2) => (_p1._value == _p2._value) ? BuildInvalid() : new ExtendedDouble(_p1._value, true, true ));
        }

        public static ExtendedDouble operator -(ExtendedDouble p1)
        {
            if (!p1.Valid)
                return BuildInvalid();
            if (p1.infinite)
                return new ExtendedDouble(-p1._value, true, true);
            return new ExtendedDouble(-p1._value, false, true);
        }

        public static ExtendedDouble operator *(ExtendedDouble p1, ExtendedDouble p2)
        {
            return ExecuteOperation(p1, p2,
             paramNoneInfinite: (_p1, _p2) => new ExtendedDouble(_p1._value * _p2._value, false, true),
             paramFirstInfiniteSecondZero: (_p1, _p2) => BuildInvalid(),
             paramFirstInfiniteSecondNonZero: (_p1, _p2) => new ExtendedDouble((_p2._value > 0) ? _p1._value : -_p1._value, true, true),
             paramFirstZeroSecondInfinite: (_p1, _p2) => BuildInvalid(),
             paramFirstNonZeroSecondInfinite: (_p1, _p2) => new ExtendedDouble((_p1._value > 0) ? _p2._value : -_p2._value, true, true),
             paramBothInfinite: (_p1, _p2) => new ExtendedDouble(_p1._value*_p2._value, true, true));
        }

        public static ExtendedDouble operator /(ExtendedDouble p1, ExtendedDouble p2)
        {
            return ExecuteOperation(p1, p2,
             paramNoneInfinite: (_p1, _p2) =>
             {
                 if (_p2._value != 0)
                     return new ExtendedDouble(_p1._value / _p2._value, false, true);
                 else if (_p1._value != 0)
                     return new ExtendedDouble((_p1._value > 0) ? 1 : -1, true, true);
                 else
                     return new ExtendedDouble(null, false, false);

             },
             paramFirstInfiniteSecondZero: (_p1, _p2) => new ExtendedDouble(_p1._value, true, true),
             paramFirstInfiniteSecondNonZero: (_p1, _p2) => new ExtendedDouble((_p2._value > 0) ? _p1._value : -_p1._value, true, true),
             paramFirstZeroSecondInfinite: (_p1, _p2) => new ExtendedDouble(0, false, true),
             paramFirstNonZeroSecondInfinite: (_p1, _p2) => new ExtendedDouble(0, false, true),
             paramBothInfinite: (_p1, _p2) => BuildInvalid());
        }

        public static bool operator ==(ExtendedDouble p1, ExtendedDouble p2)
        {
            if(!(p1.Valid && p2.Valid)) throw new InvalidOperationException();
            return (p1._value == p2._value) && (p1.infinite == p2.infinite);
        }

        public static bool operator !=(ExtendedDouble p1, ExtendedDouble p2)
        {
            if (!(p1.Valid && p2.Valid)) throw new InvalidOperationException();
            return (p1._value != p2._value) || (p1.infinite != p2.infinite);
        }

        public static bool operator >=(ExtendedDouble p1, ExtendedDouble p2)
        {
            if (!(p1.Valid && p2.Valid)) throw new InvalidOperationException();
            return (((p1._value >= p2._value) && p1.infinite && p2.infinite) ||
                    ((p1._value > 0) && p1.infinite && !p2.infinite) ||
                    ((0 > p2._value) && !p1.infinite && p2.infinite) ||
                    ((p1._value >= p2._value) && !p1.infinite && !p2.infinite));
        }

        public static bool operator >(ExtendedDouble p1, ExtendedDouble p2)
        {
            if (!(p1.Valid && p2.Valid)) throw new InvalidOperationException();
            return (((p1._value > p2._value) && p1.infinite && p2.infinite) ||
                    ((p1._value > 0) && p1.infinite && !p2.infinite) ||
                    ((0 > p2._value) && !p1.infinite && p2.infinite) ||
                    ((p1._value > p2._value) && !p1.infinite && !p2.infinite));
        }

        public static bool operator <=(ExtendedDouble p1, ExtendedDouble p2)
        {
            if (!(p1.Valid && p2.Valid)) throw new InvalidOperationException();
            return (((p1._value <= p2._value) && p1.infinite && p2.infinite) ||
                    ((p1._value < 0) && p1.infinite && !p2.infinite) ||
                    ((0 < p2._value) && !p1.infinite && p2.infinite) ||
                    ((p1._value <= p2._value) && !p1.infinite && !p2.infinite));
        }

        public static bool operator <(ExtendedDouble p1, ExtendedDouble p2)
        {
            if (!(p1.Valid && p2.Valid)) throw new InvalidOperationException();
            return (((p1._value < p2._value) && p1.infinite && p2.infinite) ||
                    ((p1._value < 0) && p1.infinite && !p2.infinite) ||
                    ((0 < p2._value) && !p1.infinite && p2.infinite) ||
                    ((p1._value < p2._value) && !p1.infinite && !p2.infinite));
        }

        public static implicit operator ExtendedDouble(double d) => new ExtendedDouble(d, false, true);

        public override bool Equals(object obj)
        {
            if(!(obj is ExtendedDouble)) return false;
            return this == ((ExtendedDouble)obj);
        }

        public override int GetHashCode()
        {
            return $"{_value?.ToString()??"null"}{infinite}{Valid}".GetHashCode();
        }
    }
}
