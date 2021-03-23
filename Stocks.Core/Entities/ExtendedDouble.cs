using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryOperator = System.Func<Stocks.Core.Entities.ExtendedDouble, Stocks.Core.Entities.ExtendedDouble, Stocks.Core.Entities.ExtendedDouble>;

namespace Stocks.Core.Entities
{
    public class ExtendedDouble
    {
        private readonly double? value;
        private readonly bool valid = false;
        private readonly bool infinite = false;
        private ExtendedDouble(double? paramValue, bool paramValid, bool paramInfinite)
        {
            throw new NotImplementedException();
        }

        public ExtendedDouble(double paramValue)
        {
            value = paramValue;
            valid = true;
            infinite = false;
        }

        public static ExtendedDouble BuildInvalid()
        {
            return new ExtendedDouble(null, false, false);
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
            if (!(p1.valid && p2.valid)) return BuildInvalid();
            if (!(p1.infinite || p2.infinite)) return paramNoneInfinite(p1, p2);
            if (p1.infinite && p2.infinite) return paramBothInfinite(p1, p2);
            var p2Zero = (!p2.infinite) && (p2.value == 0);
            if (p1.infinite && p2Zero) return paramFirstInfiniteSecondZero(p1, p2);
            if (p1.infinite && !p2Zero) return paramFirstInfiniteSecondNonZero(p1, p2);
            var p1Zero = (!p1.infinite) && (p1.value == 0);
            if (p2.infinite && p1Zero) return paramFirstZeroSecondInfinite(p1, p2);
            if (p2.infinite && !p1Zero) return paramFirstNonZeroSecondInfinite(p1, p2);
            return BuildInvalid();
        }

        public static ExtendedDouble operator +(ExtendedDouble p1, ExtendedDouble p2)
        {
            return ExecuteOperation(p1, p2,
             paramNoneInfinite: (_p1, _p2) => new ExtendedDouble() { value = _p1.value + _p2.value, infinite = false, valid = true },
             paramFirstInfiniteSecondZero: (_p1, _p2) => new ExtendedDouble() { value = _p1.value, infinite = true, valid = true },
             paramFirstInfiniteSecondNonZero: (_p1, _p2) => new ExtendedDouble() { value = _p1.value, infinite = true, valid = true },
             paramFirstZeroSecondInfinite: (_p1, _p2) => new ExtendedDouble() { value = _p2.value, infinite = true, valid = true },
             paramFirstNonZeroSecondInfinite: (_p1, _p2) => new ExtendedDouble() { value = _p2.value, infinite = true, valid = true },
             paramBothInfinite: (_p1, _p2) => (_p1.value != _p2.value) ? BuildInvalid() : new ExtendedDouble() { value = _p1.value, infinite = true, valid = true });
        }

        public static ExtendedDouble operator -(ExtendedDouble p1, ExtendedDouble p2)
        {
            return ExecuteOperation(p1, p2,
             paramNoneInfinite: (_p1, _p2) => new ExtendedDouble() { value = _p1.value - _p2.value, infinite = false, valid = true },
             paramFirstInfiniteSecondZero: (_p1, _p2) => new ExtendedDouble() { value = _p1.value, infinite = true, valid = true },
             paramFirstInfiniteSecondNonZero: (_p1, _p2) => new ExtendedDouble() { value = _p1.value, infinite = true, valid = true },
             paramFirstZeroSecondInfinite: (_p1, _p2) => new ExtendedDouble() { value = -_p2.value, infinite = true, valid = true },
             paramFirstNonZeroSecondInfinite: (_p1, _p2) => new ExtendedDouble() { value = -_p2.value, infinite = true, valid = true },
             paramBothInfinite: (_p1, _p2) => (_p1.value == _p2.value) ? BuildInvalid() : new ExtendedDouble() { value = _p1.value, infinite = true, valid = true });
        }

        public static ExtendedDouble operator -(ExtendedDouble p1)
        {
            if (!p1.valid)
                return BuildInvalid();
            if (p1.infinite)
                return new ExtendedDouble() { value = -p1.value, infinite = true, valid = true };
            return new ExtendedDouble() { value = -p1.value, infinite = false, valid = true };
        }

        public static ExtendedDouble operator *(ExtendedDouble p1, ExtendedDouble p2)
        {
            return ExecuteOperation(p1, p2,
             paramNoneInfinite: (_p1, _p2) => new ExtendedDouble() { value = _p1.value * _p2.value, infinite = false, valid = true },
             paramFirstInfiniteSecondZero: (_p1, _p2) => BuildInvalid(),
             paramFirstInfiniteSecondNonZero: (_p1, _p2) => new ExtendedDouble() { value = (_p2.value > 0) ? _p1.value : -_p1.value, infinite = true, valid = true },
             paramFirstZeroSecondInfinite: (_p1, _p2) => BuildInvalid(),
             paramFirstNonZeroSecondInfinite: (_p1, _p2) => new ExtendedDouble() { value = (_p1.value > 0) ? _p2.value : -_p2.value, infinite = true, valid = true },
             paramBothInfinite: (_p1, _p2) => new ExtendedDouble() { value = _p1.value*_p2.value, infinite = true, valid = true });
        }

        public static ExtendedDouble operator /(ExtendedDouble p1, ExtendedDouble p2)
        {
            return ExecuteOperation(p1, p2,
             paramNoneInfinite: (_p1, _p2) =>
             {
                 if (_p2.value != 0)
                     return new ExtendedDouble() { value = _p1.value / _p2.value, infinite = false, valid = true };
                 else if (_p1.value != 0)
                     return new ExtendedDouble() { value = (_p1.value > 0) ? 1 : -1, infinite = true, valid = true };
                 else
                     return new ExtendedDouble() { value = null, infinite = false, valid = false };

             },
             paramFirstInfiniteSecondZero: (_p1, _p2) => new ExtendedDouble() { value = _p1.value, infinite = true, valid = true },
             paramFirstInfiniteSecondNonZero: (_p1, _p2) => new ExtendedDouble() { value = (_p2.value > 0) ? _p1.value : -_p1.value, infinite = true, valid = true },
             paramFirstZeroSecondInfinite: (_p1, _p2) => new ExtendedDouble() { value = 0, infinite = false, valid = true },
             paramFirstNonZeroSecondInfinite: (_p1, _p2) => new ExtendedDouble() { value = 0, infinite = false, valid = true },
             paramBothInfinite: (_p1, _p2) => BuildInvalid());
        }

        public static bool operator ==(ExtendedDouble p1, ExtendedDouble p2)
        {
            return !p1.equal(p1.Value, p2.Value);
        }

        public static bool operator !=(ExtendedDouble p1, ExtendedDouble p2)
        {
            return !p1.equal(p1.Value, p2.Value);
        }

        public static bool operator >=(ExtendedDouble p1, ExtendedDouble p2)
        {
            return p1.greaterEqualThan(p1.Value, p2.Value);
        }

        public static bool operator >(ExtendedDouble p1, ExtendedDouble p2)
        {
            return p1.greaterThan(p1.Value, p2.Value);
        }

        public static bool operator <=(ExtendedDouble p1, ExtendedDouble p2)
        {
            return p1.lessEqualThan(p1.Value, p2.Value);
        }

        public static bool operator <(ExtendedDouble p1, ExtendedDouble p2)
        {
            return p1.lessThan(p1.Value, p2.Value);
        }
    }
}
