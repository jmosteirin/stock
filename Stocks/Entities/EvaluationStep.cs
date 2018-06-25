using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Entities
{
    public class EvaluationStep
    {
        public EIndex Stock { get; set; }
        public bool Buy { get; set; }
        public bool Action { get; set; }

        public override string ToString()
        {
            if (!Action)
                return @"Mantenerse";
            else
            {
                if (Buy)
                {
                    return @"Comprar " + Stock.ToString();
                }
                else
                {
                    return @"Vender " + Stock.ToString();
                }
            }
        }
    }
}
