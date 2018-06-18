using Stocks.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Algorithm
{
    public class AlgorithmConfiguration
    {
        public AlgorithmConfiguration()
        {
            Data = new Dictionary<string, double[]>();
        }
        public Dictionary<string, double[]> Data { get; set; }
        public AlgorithmConfiguration Combine(AlgorithmConfiguration paramOther, double paramRatio = 0.5)
        {
            var result = new AlgorithmConfiguration();
            var module = 0.0;
            foreach (var key in this.Data.Keys)
            {
                if ((paramOther.Data.Keys.Contains(key)) && (this.Data[key].Length == paramOther.Data[key].Length))
                {
                    result.Data[key] = new double[this.Data[key].Length];
                    for (int i = 0; i < this.Data[key].Length; i++)
                        result.Data[key][i] = (1 - paramRatio) * this.Data[key][i] + paramRatio * paramOther.Data[key][i];
                }
                module += Math.Pow(result.Data[key][0], 2.0);
            }
            module = Math.Sqrt(module);
            return result;
        }

        public double EvaluateMoneyEarnedInLast(IEnumerable<Sample> paramMidPoints, IEnumerable<Sample> paramHighPoints, IEnumerable<Sample> paramLowPoints, int paramDays = 100, double paramInitialAmount = 1000.0, double paramValueToBuy = 0.4, double paramValueToSell = -0.4)
        {
            var indicators = new Dictionary<string, IIndicator>();

            foreach (var key in Data.Keys)
            {
                indicators[key] = IndicatorFactory.Build(key);
                var extraDimensions = Data[key].Skip(1).ToArray();
                if (extraDimensions.Any())
                    for (int i = 0; i < extraDimensions.Length; i++)
                        indicators[key].SetExtraDimension(i, extraDimensions[i]);
            }

            IEnumerable<Sample> eval = Evaluate(indicators, paramMidPoints, paramHighPoints, paramLowPoints);

            var evalData = eval.Skip(paramMidPoints.Count() - paramDays).ToArray();
            var checkData = paramMidPoints.Skip(paramMidPoints.Count() - paramDays).ToArray();
            var stockValue = 0.0;
            var numOfStocks = 0.0;
            var wallet = paramInitialAmount;
            for (var day = 0; day < checkData.Length; day++)
            {
                if ((evalData[day].Value > paramValueToBuy) && (numOfStocks == 0))
                {
                    //Buy!!
                    stockValue = checkData[day].Value;
                    numOfStocks = wallet / stockValue;
                    wallet = 0.0;
                }
                else if((evalData[day].Value < paramValueToSell) && (numOfStocks != 0))
                {
                    //Sell!!
                    wallet = checkData[day].Value * numOfStocks;
                    stockValue = 0.0;
                    numOfStocks = 0.0;
                }
            }

            return (wallet + (stockValue * numOfStocks)) / 1000.0;
        }

        private IEnumerable<Sample> Evaluate(Dictionary<string, IIndicator> indicators, IEnumerable<Sample> paramMidPoints, IEnumerable<Sample> paramHighPoints, IEnumerable<Sample> paramLowPoints)
        {
            IEnumerable<Sample> returned = null;
            foreach (var key in indicators.Keys)
            {
                var indicator = indicators[key];
                var contribution = indicator.GetValue(paramMidPoints, paramHighPoints, paramLowPoints).ScalarMultiplication(Data[key][0]);
                if (returned == null)
                    returned = contribution;
                else
                    returned = returned.Add(contribution);
            }
            return returned;
        }
    }
}
