using Newtonsoft.Json;
using Stocks.Algorithm;
using Stocks.Entities;
using Stocks.Indicators;
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
            var candles = 
                new StockInformation(
                    BuildStockInformationForIndex(Entities.EIndex.Acciona),
                    BuildStockInformationForIndex(Entities.EIndex.Acerinox),
                    BuildStockInformationForIndex(Entities.EIndex.ACS),
                    BuildStockInformationForIndex(Entities.EIndex.AmadeusIT),
                    BuildStockInformationForIndex(Entities.EIndex.ArcelorMittal),
                    BuildStockInformationForIndex(Entities.EIndex.BancoSabadell),
                    BuildStockInformationForIndex(Entities.EIndex.BancoSantander),
                    BuildStockInformationForIndex(Entities.EIndex.Bankia),
                    BuildStockInformationForIndex(Entities.EIndex.Bankinter),
                    BuildStockInformationForIndex(Entities.EIndex.BBVA),
                    BuildStockInformationForIndex(Entities.EIndex.Caixabank),
                    BuildStockInformationForIndex(Entities.EIndex.CieAutomotive),
                    BuildStockInformationForIndex(Entities.EIndex.Colonial),
                    BuildStockInformationForIndex(Entities.EIndex.Enagas),
                    BuildStockInformationForIndex(Entities.EIndex.Endesa),
                    BuildStockInformationForIndex(Entities.EIndex.Ferrovial),
                    BuildStockInformationForIndex(Entities.EIndex.GasNatural),
                    BuildStockInformationForIndex(Entities.EIndex.Grifols),
                    BuildStockInformationForIndex(Entities.EIndex.Iberdrola),
                    BuildStockInformationForIndex(Entities.EIndex.Inditex),
                    BuildStockInformationForIndex(Entities.EIndex.Indra),
                    BuildStockInformationForIndex(Entities.EIndex.Mapfre),
                    BuildStockInformationForIndex(Entities.EIndex.Mediaset),
                    BuildStockInformationForIndex(Entities.EIndex.MeliaHotels),
                    BuildStockInformationForIndex(Entities.EIndex.MerlinEntertainments),
                    BuildStockInformationForIndex(Entities.EIndex.RedElectrica),
                    BuildStockInformationForIndex(Entities.EIndex.Repsol),
                    BuildStockInformationForIndex(Entities.EIndex.SiemensGamesa),
                    BuildStockInformationForIndex(Entities.EIndex.TecnicasReunidas),
                    BuildStockInformationForIndex(Entities.EIndex.Telefonica),
                    BuildStockInformationForIndex(Entities.EIndex.Viscofan));

            AlgorithmConfiguration[] algorithms = InitAlgorithms();

            for (int i = 0; i < 200; i++)
            {
                algorithms = SortAlgorithms(candles, algorithms);
                algorithms = CombineAlgorithms(algorithms);
            }

            PrintAlgorithms(candles, algorithms);
        }

        private StockInformationForIndex BuildStockInformationForIndex(EIndex paramIndex)
        {
            return new StockInformationForIndex((int)paramIndex,
                        investingContext.GetCandles(500, paramIndex).ToArray());
        }

        private void PrintAlgorithms(StockInformation paramCandles, AlgorithmConfiguration[] paramAlgorithms)
        {
            foreach (var amount in paramAlgorithms.Select(a =>
            {
                StringBuilder builder = new StringBuilder();
                foreach (var indicator in a.Data)
                    builder.AppendFormat(@"{0}:{1}% ", indicator.Key, Math.Round(indicator.Value[0] * 100, 2));
                builder.AppendFormat(@"{0}:{1}% ", @"threshold to sell", Math.Round(a.ThresholdToSell, 2));
                builder.AppendFormat(@"{0}:{1}% ", @"threshold to buy", Math.Round(a.ThresholdToBuy, 2));
                return builder.Append(a.EvaluateMoneyEarnedInLast(paramCandles).ProfitRatio);
            }))
                Console.WriteLine(amount);
            var winner = paramAlgorithms.First();
            foreach(var step in winner.EvaluateMoneyEarnedInLast(paramCandles).Steps)
                Console.WriteLine(step.ToString());
            Console.Write(@"Fortune teller: ");
            Console.WriteLine(GetFortuneTellerRatio(paramCandles));
        }

        private double GetFortuneTellerRatio(StockInformation paramStockInformation, int paramDays = 100)
        {
            var numOfSamplesToSkip = paramStockInformation.StockInformationForIndexes.First().MidPoints.Count() - paramDays;

            var indexesEvaluation = paramStockInformation.StockInformationForIndexes.
                Select(s => new
                {
                    CheckData = s.MidPoints.Skip(numOfSamplesToSkip).ToArray()
                }).ToArray();

            var returned = 1.0;
            for (var day = 1; day < paramDays; day++)
            {
                returned *= indexesEvaluation.Select(i => i.CheckData[day].Value / i.CheckData[day - 1].Value).Max();
            }
            return returned;
        }

        private AlgorithmConfiguration[] CombineAlgorithms(AlgorithmConfiguration[] paramAlgorithms)
        {
            var newAlgorithms = new AlgorithmConfiguration[10];
            newAlgorithms[0] = paramAlgorithms[0].Combine(paramAlgorithms[1], 0.1);
            newAlgorithms[1] = paramAlgorithms[0].Combine(paramAlgorithms[1], 0.25);
            newAlgorithms[2] = paramAlgorithms[0].Combine(paramAlgorithms[1], 0.5);
            newAlgorithms[3] = paramAlgorithms[0].Combine(paramAlgorithms[1], 0.9);
            newAlgorithms[4] = paramAlgorithms[1].Combine(paramAlgorithms[2], 0.5);
            newAlgorithms[5] = paramAlgorithms[0].Combine(paramAlgorithms[2], 0.5);
            newAlgorithms[6] = paramAlgorithms[0].Combine(GenerateRandomAlgorithConfiguration(), 0.2);
            newAlgorithms[7] = paramAlgorithms[0].Combine(GenerateRandomAlgorithConfiguration(), 0.4);
            newAlgorithms[8] = GenerateRandomAlgorithConfiguration();
            newAlgorithms[9] = GenerateRandomAlgorithConfiguration();
            return newAlgorithms;
        }

        private AlgorithmConfiguration[] SortAlgorithms(StockInformation paramCandles, AlgorithmConfiguration[] paramAlgorithms)
        {
            return (new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }).Select(k => new
            {
                Algorithm = paramAlgorithms[k],
                Ratio = paramAlgorithms[k].EvaluateMoneyEarnedInLast(paramCandles)
            }).OrderByDescending(e => e.Ratio.ProfitRatio).Select(e => e.Algorithm).ToArray();
        }

        private AlgorithmConfiguration[] InitAlgorithms()
        {
            var temp = new List<AlgorithmConfiguration>();
            for (int i = 0; i < 10; i++)
                temp.Add(GenerateRandomAlgorithConfiguration());
            return temp.ToArray();
        }

        private AlgorithmConfiguration GenerateRandomAlgorithConfiguration()
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
            ratio = (1.0 - accumulated) * random.NextDouble();
            algorithm.Data[Constants.Stability] = new double[] { ratio };
            accumulated += ratio;
            algorithm.Data[Constants.RSI] = new double[] { (1.0 - accumulated) };
            algorithm.ThresholdToBuy = random.NextDouble();
            algorithm.ThresholdToSell = -random.NextDouble();
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

        public IEnumerable<Sample> Eval(int paramIndex, double paramBollinger, double paramMACD, double paramRSI)
        {
            var algorithm = new AlgorithmConfiguration();
            algorithm.Data[Constants.Bollinger] = new double[] { paramBollinger, 1.0 };
            algorithm.Data[Constants.MACD] = new double[] { paramMACD, 0.2 };
            algorithm.Data[Constants.RSI] = new double[] { paramRSI };

            var stockInfo = new StockInformationForIndex(paramIndex, investingContext.GetCandles(500, (Entities.EIndex)paramIndex).ToArray());
            return algorithm.Evaluate(algorithm.GenerateIndicators(), stockInfo.MidPoints, stockInfo.HighPoints, stockInfo.LowPoints);
        }
        public IEnumerable<Sample> EvalIndicator(string paramIndicator, int paramIndex)
        {
            var algorithm = new AlgorithmConfiguration();
            algorithm.Data[Constants.Bollinger] = new double[] { 1.0, 1.0 };
            algorithm.Data[Constants.MACD] = new double[] { 1.0, 0.2 };
            algorithm.Data[Constants.RSI] = new double[] { 1.0 };

            var stockInfo = new StockInformationForIndex(paramIndex, investingContext.GetCandles(500, (Entities.EIndex)paramIndex).ToArray());
            return algorithm.Evaluate(algorithm.GenerateIndicators().Where(i => i.Key == paramIndicator).ToDictionary(p => p.Key, p => p.Value), stockInfo.MidPoints, stockInfo.HighPoints, stockInfo.LowPoints);
        }
    }
}
