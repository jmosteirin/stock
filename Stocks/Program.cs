using Stocks.Algorithm;
using Stocks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks
{
    class Program
    {
        static void Main(string[] args)
        {
            //IInvestingContext investingContext = new InvestingContext();
            //var candles = investingContext.GetCandles(500, Entities.EIndex.DowJones);
            //var midPoints = candles.Select(c => new SingleValueSample<double>() { Date = c.Date, Valid = true, InternalValue = (c.High + c.Low) / 2.0 }).ToArray();
            //var highPoints = candles.Select(c => new SingleValueSample<double>() { Date = c.Date, Valid = true, InternalValue = c.High }).ToArray();
            //var lowPoints = candles.Select(c => new SingleValueSample<double>() { Date = c.Date, Valid = true, InternalValue = c.Low }).ToArray();

            //var temp = new List<AlgorithmConfiguration>();
            //for (int i = 0; i < 8; i++)
            //    temp.Add(GenerateRandomAlgorithConfiguration());
            //var algorithms = temp.ToArray();

            //for (int i = 0; i < 15; i++)
            //{
            //    algorithms = (new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }).Select(k => new
            //    {
            //        Algorithm = algorithms[k],
            //        Ratio = algorithms[k].EvaluateMoneyEarnedInLast(midPoints, highPoints, lowPoints)
            //    }).OrderByDescending(e => e.Ratio).Select(e => e.Algorithm).ToArray();
            //    var newAlgorithms = new AlgorithmConfiguration[8];
            //    newAlgorithms[0] = algorithms[0].Combine(algorithms[1], 0.1);
            //    newAlgorithms[1] = algorithms[0].Combine(algorithms[1], 0.25);
            //    newAlgorithms[2] = algorithms[0].Combine(algorithms[1], 0.5);
            //    newAlgorithms[3] = algorithms[0].Combine(algorithms[1], 0.9);
            //    newAlgorithms[4] = algorithms[1].Combine(algorithms[2], 0.5);
            //    newAlgorithms[5] = algorithms[0].Combine(algorithms[2], 0.5);
            //    newAlgorithms[6] = GenerateRandomAlgorithConfiguration();
            //    newAlgorithms[7] = GenerateRandomAlgorithConfiguration();
            //    algorithms = newAlgorithms;
            //}


            //foreach(var amount in algorithms.Select(a =>
            //{
            //    StringBuilder builder = new StringBuilder();
            //    foreach (var indicator in a.Data)
            //        builder.AppendFormat(@"{0}:{1}% ", indicator.Key, Math.Round(indicator.Value[0] * 100, 2));
            //    return builder.Append(a.EvaluateMoneyEarnedInLast(midPoints, highPoints, lowPoints));
            //}))
            //    Console.WriteLine(amount);

            //Console.ReadKey();
        }

        private static AlgorithmConfiguration GenerateRandomAlgorithConfiguration()
        {
            var random = new Random();
            var algorithm = new AlgorithmConfiguration();
            var accumulated = 0.0;
            var ratio = random.NextDouble();
            algorithm.Data[IndicatorConstants.Bollinger] = new double[] { ratio, 1.0 };
            accumulated += ratio;
            ratio = (1.0 - accumulated) * random.NextDouble();
            algorithm.Data[IndicatorConstants.MACD] = new double[] { ratio, 0.2 };
            accumulated += ratio;
            algorithm.Data[IndicatorConstants.RSI] = new double[] { (1.0 - accumulated) };
            return algorithm;
        }
    }
}
