using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Entities
{
    public class EvaluationResult
    {
        public double FinalMoney { get; set; }
        public double InitialMoney { get; set; }
        public double ProfitRatio { get; set; }
        public IEnumerable<EvaluationStep> Steps { get; set; }
    }
}
