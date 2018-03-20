using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Indicators
{
    public class Sample
    {
        public Sample()
        {
        }
        public Sample(double paramValue, DateTime paramDate)
        {
            Value = paramValue;
            Date = paramDate;
            Valid = true;
        }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public bool Valid { get; set; }
        private bool infinite = false;
        public bool Infinite
        {
            get
            {
                return infinite;
            }
            set
            {
                infinite = value;
                Value = GetSign(Value);
            }
        }
        public Sample Subtract(Sample paramOther, DateTime paramDate = default(DateTime))
        {
            if (!Valid || !paramOther.Valid)
                return new Sample()
                {
                    Valid = false,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate
                };

            if (!Infinite && !paramOther.Infinite)
                return new Sample()
                {
                    Value = Value - paramOther.Value,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate,
                    Valid = true,
                    Infinite = false
                };
            else if (Infinite && paramOther.Infinite)
            {
                if(Value != paramOther.Value)
                    return new Sample()
                    {
                        Valid = true,
                        Value = Value,
                        Infinite = true,
                        Date = (paramDate == default(DateTime)) ? Date : paramDate
                    };

                else
                    return new Sample()
                    {
                        Valid = false,
                        Date = (paramDate == default(DateTime)) ? Date : paramDate
                    };
            }
            else if (Infinite)
            {
                return new Sample()
                {
                    Value = Value,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate,
                    Valid = true,
                    Infinite = true
                };
            }
            else if (paramOther.Infinite)
            {
                return new Sample()
                {
                    Value = -paramOther.Value,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate,
                    Valid = true,
                    Infinite = true
                };
            }
            return new Sample()
            {
                Valid = false,
                Date = (paramDate == default(DateTime)) ? Date : paramDate
            };
        }
        public Sample Add(Sample paramOther, DateTime paramDate = default(DateTime))
        {
            if (!Valid || !paramOther.Valid)
                return new Sample()
                {
                    Valid = false,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate
                };

            if (!Infinite && !paramOther.Infinite)
                return new Sample()
                {
                    Value = Value + paramOther.Value,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate,
                    Valid = true,
                    Infinite = false
                };
            else if (Infinite && paramOther.Infinite)
            {
                if (Value == paramOther.Value)
                    return new Sample()
                    {
                        Valid = true,
                        Value = Value,
                        Infinite = true,
                        Date = (paramDate == default(DateTime)) ? Date : paramDate
                    };

                else
                    return new Sample()
                    {
                        Valid = false,
                        Date = (paramDate == default(DateTime)) ? Date : paramDate
                    };
            }
            else if (Infinite)
            {
                return new Sample()
                {
                    Value = Value,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate,
                    Valid = true,
                    Infinite = true
                };
            }
            else if (paramOther.Infinite)
            {
                return new Sample()
                {
                    Value = paramOther.Value,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate,
                    Valid = true,
                    Infinite = true
                };
            }
            return new Sample()
            {
                Valid = false,
                Date = (paramDate == default(DateTime)) ? Date : paramDate
            };
        }
        public Sample Multiply(Sample paramOther, DateTime paramDate = default(DateTime))
        {
            if (!Valid || !paramOther.Valid)
                return new Sample()
                {
                    Valid = false,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate
                };

            if (!Infinite && !paramOther.Infinite)
                return new Sample()
                {
                    Value = Value * paramOther.Value,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate,
                    Valid = true,
                    Infinite = false
                };
            else if (Infinite && paramOther.Infinite)
            {
                if (Value == paramOther.Value)
                    return new Sample()
                    {
                        Valid = true,
                        Value = 1.0,
                        Infinite = true,
                        Date = (paramDate == default(DateTime)) ? Date : paramDate
                    };

                else
                    return new Sample()
                    {
                        Valid = true,
                        Value = -1.0,
                        Infinite = true,
                        Date = (paramDate == default(DateTime)) ? Date : paramDate
                    };
            }
            else if (Infinite)
            {
                if (paramOther.Value == 0)
                    return new Sample()
                    {
                        Value = 0,
                        Date = (paramDate == default(DateTime)) ? Date : paramDate,
                        Valid = true,
                        Infinite = false
                    };
                else
                    return new Sample()
                    {
                        Value = GetMultiplicationSign(Value, paramOther.Value),
                        Date = (paramDate == default(DateTime)) ? Date : paramDate,
                        Valid = true,
                        Infinite = true
                    };
            }
            else if (paramOther.Infinite)
            {
                if (Value == 0)
                    return new Sample()
                    {
                        Value = 0,
                        Date = (paramDate == default(DateTime)) ? Date : paramDate,
                        Valid = true,
                        Infinite = false
                    };
                else
                    return new Sample()
                    {
                        Value = GetMultiplicationSign(Value, paramOther.Value),
                        Date = (paramDate == default(DateTime)) ? Date : paramDate,
                        Valid = true,
                        Infinite = true
                    };
            }
            return new Sample()
            {
                Valid = false,
                Date = (paramDate == default(DateTime)) ? Date : paramDate
            };
        }
        public Sample Multiply(double paramScalar, DateTime paramDate = default(DateTime))
        {
            if (!Valid)
                return new Sample()
                {
                    Valid = false,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate
                };

            if (Infinite)
            {
                if (paramScalar == 0)
                    return new Sample()
                    {
                        Value = 0,
                        Date = (paramDate == default(DateTime)) ? Date : paramDate,
                        Valid = true,
                        Infinite = false
                    };
                else
                    return new Sample()
                    {
                        Value = GetMultiplicationSign(Value, paramScalar),
                        Date = (paramDate == default(DateTime)) ? Date : paramDate,
                        Valid = true,
                        Infinite = true
                    };
            }
            else
            {
                return new Sample()
                    {
                        Value = Value*paramScalar,
                        Date = (paramDate == default(DateTime)) ? Date : paramDate,
                        Valid = true,
                        Infinite = false
                    };
            }
        }
        public Sample Divide(Sample paramOther, DateTime paramDate = default(DateTime))
        {
            if (!Valid || !paramOther.Valid)
                return new Sample()
                {
                    Valid = false,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate
                };

            if (!Infinite && !paramOther.Infinite)
            {
                if ((paramOther.Value == 0) && (Value == 0))
                    return new Sample()
                    {
                        Date = (paramDate == default(DateTime)) ? Date : paramDate,
                        Valid = false
                    };
                else if(paramOther.Value == 0)
                    return new Sample()
                    {
                        Value = GetSign(Value),
                        Date = (paramDate == default(DateTime)) ? Date : paramDate,
                        Valid = true,
                        Infinite = true
                    };
                else
                    return new Sample()
                    {
                        Value = Value / paramOther.Value,
                        Date = (paramDate == default(DateTime)) ? Date : paramDate,
                        Valid = true,
                        Infinite = false
                    };
            }
            else if (Infinite && paramOther.Infinite)
            {
                return new Sample()
                {
                    Valid = false,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate
                };
            }
            else if (Infinite)
            {
                if(paramOther.Value == 0)
                    return new Sample()
                    {
                        Value = Value,
                        Date = (paramDate == default(DateTime)) ? Date : paramDate,
                        Valid = true,
                        Infinite = true
                    };
                else
                    return new Sample()
                    {
                        Value = GetMultiplicationSign(Value, paramOther.Value),
                        Date = (paramDate == default(DateTime)) ? Date : paramDate,
                        Valid = true,
                        Infinite = true
                    };
            }
            else if (paramOther.Infinite)
            {
                return new Sample()
                {
                    Value = 0,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate,
                    Valid = true,
                    Infinite = false
                };
            }
            return new Sample()
            {
                Valid = false,
                Date = (paramDate == default(DateTime)) ? Date : paramDate
            };
        }
        public Sample Square(DateTime paramDate = default(DateTime))
        {
            if (!Valid)
                return new Sample()
                {
                    Valid = false,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate
                };

            if (Infinite)
            {
                return new Sample()
                {
                    Value = 1.0,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate,
                    Valid = true,
                    Infinite = true
                };
            }
            else
            {
                return new Sample()
                {
                    Value = Value * Value,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate,
                    Valid = true,
                    Infinite = false
                };
            }
        }
        public Sample SquareRoot(DateTime paramDate = default(DateTime))
        {
            if (!Valid || (Value < 0))
                return new Sample()
                {
                    Valid = false,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate
                };

            if (Infinite)
            {
                return new Sample()
                {
                    Value = Value,
                    Date = (paramDate == default(DateTime)) ? Date : paramDate,
                    Valid = true,
                    Infinite = true
                };
            }
            else
            {
                return new Sample()
                {
                    Value = Math.Sqrt(Value),
                    Date = (paramDate == default(DateTime)) ? Date : paramDate,
                    Valid = true,
                    Infinite = false
                };
            }
        }
        public Sample Clone()
        {
            return new Sample()
            {
                Value = Value,
                Infinite = Infinite,
                Valid = Valid,
                Date = Date
            };
        }
        private double GetMultiplicationSign(double paramFirst, double paramSecond)
        {
            return (((paramFirst > 0) && (paramSecond > 0)) || ((paramFirst < 0) && (paramSecond < 0))) ? 1.0 : -1.0;
        }
        private double GetSign(double paramNum)
        {
            return (paramNum >= 0) ? 1.0 : -1.0;
        }
    }
}
