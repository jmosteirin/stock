using Newtonsoft.Json;
using Stocks.Algorithm;
using Stocks.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks
{
    class StockLogic : IStockLogic
    {
        IInvestingContext investingContext = null;

        public StockLogic(IInvestingContext paramInvestingContext)
        {
            if (paramInvestingContext == null)
                throw new ArgumentNullException(@"paramInvestingContext");

            investingContext = paramInvestingContext;
        }

        public void LetsBecomeRich()
        {
            var candles = new StockInformation(investingContext.GetCandles(500, Entities.EIndex.DowJones));

            AlgorithmConfiguration[] algorithms = InitAlgorithms();

            for (int i = 0; i < 15; i++)
            {
                algorithms = SortAlgorithms(candles, algorithms);
                algorithms = CombineAlgorithms(algorithms);
            }

            PrintAlgorithms(candles, algorithms);
        }

        private static void PrintAlgorithms(StockInformation paramCandles, AlgorithmConfiguration[] paramAlgorithms)
        {
            foreach (var amount in paramAlgorithms.Select(a =>
            {
                StringBuilder builder = new StringBuilder();
                foreach (var indicator in a.Data)
                    builder.AppendFormat(@"{0}:{1}% ", indicator.Key, Math.Round(indicator.Value[0] * 100, 2));
                return builder.Append(a.EvaluateMoneyEarnedInLast(paramCandles.MidPoints, paramCandles.HighPoints, paramCandles.LowPoints));
            }))
                Console.WriteLine(amount);
        }

        private static AlgorithmConfiguration[] CombineAlgorithms(AlgorithmConfiguration[] paramAlgorithms)
        {
            var newAlgorithms = new AlgorithmConfiguration[8];
            newAlgorithms[0] = paramAlgorithms[0].Combine(paramAlgorithms[1], 0.1);
            newAlgorithms[1] = paramAlgorithms[0].Combine(paramAlgorithms[1], 0.25);
            newAlgorithms[2] = paramAlgorithms[0].Combine(paramAlgorithms[1], 0.5);
            newAlgorithms[3] = paramAlgorithms[0].Combine(paramAlgorithms[1], 0.9);
            newAlgorithms[4] = paramAlgorithms[1].Combine(paramAlgorithms[2], 0.5);
            newAlgorithms[5] = paramAlgorithms[0].Combine(paramAlgorithms[2], 0.5);
            newAlgorithms[6] = GenerateRandomAlgorithConfiguration();
            newAlgorithms[7] = GenerateRandomAlgorithConfiguration();
            return newAlgorithms;
        }

        private static AlgorithmConfiguration[] SortAlgorithms(StockInformation paramCandles, AlgorithmConfiguration[] paramAlgorithms)
        {
            return (new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }).Select(k => new
            {
                Algorithm = paramAlgorithms[k],
                Ratio = paramAlgorithms[k].EvaluateMoneyEarnedInLast(paramCandles.MidPoints, paramCandles.HighPoints, paramCandles.LowPoints)
            }).OrderByDescending(e => e.Ratio).Select(e => e.Algorithm).ToArray();
        }

        private static AlgorithmConfiguration[] InitAlgorithms()
        {
            var temp = new List<AlgorithmConfiguration>();
            for (int i = 0; i < 8; i++)
                temp.Add(GenerateRandomAlgorithConfiguration());
            return temp.ToArray();
        }

        private static AlgorithmConfiguration GenerateRandomAlgorithConfiguration()
        {
            var random = new Random();
            var algorithm = new AlgorithmConfiguration();
            var accumulated = 0.0;
            var ratio = random.NextDouble();
            algorithm.Data[Constants.Bollinger] = new double[] { ratio, 1.0 };
            accumulated += ratio;
            ratio = (1.0 - accumulated) * random.NextDouble();
            algorithm.Data[Constants.MACD] = new double[] { ratio, 0.2 };
            accumulated += ratio;
            algorithm.Data[Constants.RSI] = new double[] { (1.0 - accumulated) };
            return algorithm;
        }

        public void RefreshStoredIndexes()
        {
            var indexes = investingContext.GetIndexes(Constants.StockIndexesStaringId, Constants.StockIndexesEndingId);
            SaveIndexesToCache(indexes);
        }

        private void SaveIndexesToCache(IEnumerable<Index> paramIndexes)
        {
            using (var fileStream = new FileStream(Constants.IndexesCacheFileName, FileMode.Create, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(paramIndexes));
                }
            }
        }

        public void ExportCSV(string paramFileName)
        {
            var indexes = ReadIndexesFromCache();

            using (var fileStream = new FileStream(paramFileName, FileMode.Create, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    foreach (var index in indexes)
                        streamWriter.WriteLine(String.Format(@"{0};{1}", index.Id, index.Description));
                }
            }
        }

        private Index[] ReadIndexesFromCache()
        {
            string fileContent = String.Empty;
            using (var fileStream = new FileStream(Constants.IndexesCacheFileName, FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    fileContent = streamReader.ReadToEnd();
                }
            }
            var indexes = JsonConvert.DeserializeObject<Index[]>(fileContent);
            return indexes;
        }

        public void AddIndexesToCache(int paramStartId, int paramEndId)
        {
            var indexes = ReadIndexesFromCache().ToList();
            indexes.AddRange(investingContext.GetIndexes(paramStartId, paramEndId));
            SaveIndexesToCache(indexes);
        }
    }
}
