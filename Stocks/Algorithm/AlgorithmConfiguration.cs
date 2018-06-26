using Stocks.Entities;
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
        public double ThresholdToSell { get; set; }
        public double ThresholdToBuy { get; set; }
        public Dictionary<string, double[]> Data { get; set; }
        public AlgorithmConfiguration Combine(AlgorithmConfiguration paramOther, double paramRatio = 0.5)
        {
            var result = new AlgorithmConfiguration();
            foreach (var key in this.Data.Keys)
            {
                if ((paramOther.Data.Keys.Contains(key)) && (this.Data[key].Length == paramOther.Data[key].Length))
                {
                    result.Data[key] = new double[this.Data[key].Length];
                    for (int i = 0; i < this.Data[key].Length; i++)
                        result.Data[key][i] = (1.0 - paramRatio) * this.Data[key][i] + paramRatio * paramOther.Data[key][i];
                }
            }
            result.ThresholdToBuy = (1.0 - paramRatio) * this.ThresholdToBuy + paramRatio * paramOther.ThresholdToBuy;
            result.ThresholdToSell = (1.0 - paramRatio) * this.ThresholdToSell + paramRatio * paramOther.ThresholdToSell;
            return result;
        }

        public EvaluationResult EvaluateMoneyEarnedInLast(StockInformation paramStockInformation, int paramDays = 100, double paramInitialAmount = 1000.0)
        {
            var returned = new EvaluationResult();

            var indicators = GenerateIndicators();

            if (paramStockInformation.StockInformationForIndexes.Count() == 0)
                throw new Exception(@"No data");

            var numOfSamplesToSkip = paramStockInformation.StockInformationForIndexes.First().MidPoints.Count() - paramDays;

            var indexesEvaluation = paramStockInformation.StockInformationForIndexes.
                Select(s => new
                {
                    StockInformationForIndex = s,
                    EvalData = Evaluate(indicators, s.MidPoints, s.HighPoints, s.LowPoints).Skip(numOfSamplesToSkip).ToArray(),
                    CheckData = s.MidPoints.Skip(numOfSamplesToSkip).ToArray()
                }).ToArray();

            var stockValue = 0.0;
            var numOfStocks = 0.0;
            var stockIndex = 0;
            var wallet = paramInitialAmount;
            var evaluationSteps = new List<EvaluationStep>();
            for (var day = 1; day < paramDays; day++)
            {
                var maxEvaluation = indexesEvaluation.Max(e => e.EvalData[day-1].Value);
                var maxEvaluationIndex = indexesEvaluation.First(e => e.EvalData[day-1].Value == maxEvaluation);
                if ((maxEvaluation > ThresholdToBuy) && (numOfStocks == 0))
                {
                    //Buy!!
                    stockValue = maxEvaluationIndex.CheckData[day].Value;
                    stockIndex = maxEvaluationIndex.StockInformationForIndex.Index;
                    numOfStocks = wallet / stockValue;
                    wallet = 0.0;
                    evaluationSteps.Add(new EvaluationStep() { Action = true, Buy = true, Stock = (EIndex)stockIndex });
                }
                else if ((maxEvaluation > ThresholdToBuy) && (numOfStocks != 0) && (maxEvaluationIndex.StockInformationForIndex.Index != stockIndex))
                {
                    //Buy another stock!!
                    var currentStockEvaluation = indexesEvaluation.First(e => e.StockInformationForIndex.Index == stockIndex);
                    wallet = currentStockEvaluation.CheckData[day].Value * numOfStocks;
                    stockValue = maxEvaluationIndex.CheckData[day].Value;
                    stockIndex = maxEvaluationIndex.StockInformationForIndex.Index;
                    numOfStocks = wallet / stockValue;
                    wallet = 0.0;
                    evaluationSteps.Add(new EvaluationStep() { Action = true, Buy = true, Stock = (EIndex)stockIndex });
                }
                else
                {
                    var currentStockEvaluation = indexesEvaluation.FirstOrDefault(e => e.StockInformationForIndex.Index == stockIndex);
                    if ((currentStockEvaluation != null) && ((currentStockEvaluation.EvalData[day].Value < ThresholdToSell) && (numOfStocks != 0)))
                    {
                        //Sell!!
                        wallet = currentStockEvaluation.CheckData[day].Value * numOfStocks;
                        stockIndex = 0; 
                        stockValue = 0.0;
                        numOfStocks = 0.0;
                        evaluationSteps.Add(new EvaluationStep() { Action = true, Buy = false, Stock = (EIndex)stockIndex });
                    }
                    else
                        evaluationSteps.Add(new EvaluationStep() { Action = false });
                }
            }

            if (numOfStocks != 0)
            { 
                var lastStockEvaluation = indexesEvaluation.First(e => e.StockInformationForIndex.Index == stockIndex);
                returned.FinalMoney = wallet + (lastStockEvaluation.CheckData[paramDays - 1].Value * numOfStocks);
                returned.InitialMoney = paramInitialAmount;
                returned.ProfitRatio = returned.FinalMoney / returned.InitialMoney;
                returned.Steps = evaluationSteps;
                return returned;
            }
            returned.FinalMoney = wallet;
            returned.InitialMoney = paramInitialAmount;
            returned.ProfitRatio = returned.FinalMoney / returned.InitialMoney;
            returned.Steps = evaluationSteps;
            return returned;
        }

        public EvaluationStep Predict(StockInformation paramStockInformation, int paramCurrentIndex)
        {
            var returned = new EvaluationResult();

            var indicators = GenerateIndicators();

            if (paramStockInformation.StockInformationForIndexes.Count() == 0)
                throw new Exception(@"No data");

            var indexesEvaluation = paramStockInformation.StockInformationForIndexes.
                Select(s => new
                {
                    StockInformationForIndex = s,
                    EvalData = Evaluate(indicators, s.MidPoints, s.HighPoints, s.LowPoints).Last(),
                }).ToArray();

            var maxEvaluation = indexesEvaluation.Max(e => e.EvalData.Value);
            var maxEvaluationIndex = indexesEvaluation.First(e => e.EvalData.Value == maxEvaluation);
            if ((maxEvaluation > ThresholdToBuy) && (paramCurrentIndex == 0))
            {
                return new EvaluationStep() { Action = true, Buy = true, Stock = (EIndex)maxEvaluationIndex.StockInformationForIndex.Index };
            }
            else if ((maxEvaluation > ThresholdToBuy) && (paramCurrentIndex != 0) && (maxEvaluationIndex.StockInformationForIndex.Index != paramCurrentIndex))
            {
                return new EvaluationStep() { Action = true, Buy = true, Stock = (EIndex)maxEvaluationIndex.StockInformationForIndex.Index };
            }
            else
            {
                var currentStockEvaluation = indexesEvaluation.FirstOrDefault(e => e.StockInformationForIndex.Index == paramCurrentIndex);
                if ((currentStockEvaluation != null) && ((currentStockEvaluation.EvalData.Value < ThresholdToSell) && (paramCurrentIndex != 0)))
                {
                    return new EvaluationStep() { Action = true, Buy = false, Stock = (EIndex)maxEvaluationIndex.StockInformationForIndex.Index };
                }
                else
                    return new EvaluationStep() { Action = false };
            }
        }

        public Dictionary<string, IIndicator> GenerateIndicators()
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

            return indicators;
        }

        public IEnumerable<Sample> Evaluate(Dictionary<string, IIndicator> indicators, IEnumerable<Sample> paramMidPoints, IEnumerable<Sample> paramHighPoints, IEnumerable<Sample> paramLowPoints)
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
