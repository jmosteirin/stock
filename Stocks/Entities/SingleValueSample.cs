using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Entities
{
    public class SingleValueSample<T> : Sample
    {
        public SingleValueSample()
        {
        }
        public SingleValueSample(NumericWrapper<T> paramValue, DateTime paramDate)
        {
            Value = paramValue;
            Date = paramDate;
            Valid = true;
            Infinite = false;
        }
        public NumericWrapper<T> Value { get; set; }
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
