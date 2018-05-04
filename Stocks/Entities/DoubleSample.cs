using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Entities
{
    public class DoubleSample : SingleValueSample<double>
    {
        public DoubleSample() :
            base(NumericWrapper<double>.Build(0), DateTime.Now)
        {

        }

        public DoubleSample(double paramValue, DateTime paramDate) :
            base(NumericWrapper<double>.Build(paramValue), paramDate)
        {
        }
    }
}
