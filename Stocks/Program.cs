using CommandLine;
using Newtonsoft.Json;
using Stocks.Algorithm;
using Stocks.Entities;
using Stocks.Indicators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Stocks
{
    class Program
    {
        static IInvestingContext investingContext = new InvestingContext();
        static IStockLogic stockLogic = new StockLogic(investingContext);

        [Verb("advice", HelpText = "Advices which operation to do.")]
        class AdviceOptions
        {
            [Option('i', @"index", Default = 0, Required = false)]
            public int Index { get; set; }
        }
        [Verb("list-indexes", HelpText = "Lists all the indexes available.")]
        class ListIndexesOptions
        {
        }
        [Verb("show-candles", HelpText = "Shows the candles for a particular index.")]
        class ShowCandlesOptions
        {
            [Option('i', @"index")]
            public int Index { get; set; }
        }

        [Verb("eval", HelpText = "Eval stock.")]
        class EvalOptions
        {
            [Option('i', @"index")]
            public int Index { get; set; }
            [Option('b', @"bollinger")]
            public double Bollinger { get; set; }
            [Option('m', @"macd")]
            public double MACD { get; set; }
            [Option('r', @"rsi")]
            public double RSI { get; set; }
        }

        [Verb("rich", HelpText = "Lets get rich.")]
        class RichOptions
        {
        }

        [Verb("refresh", HelpText = "Refresh the indexes cache.")]
        class RefreshCachedIndexesOptions
        {
        }

        [Verb("add-indexes", HelpText = "Adds some extra indexes to cache.")]
        class AddIndexesToCacheOptions
        {
            [Option('s', @"start")]
            public int StartId { get; set; }
            [Option('e', @"end")]
            public int EndId { get; set; }
        }

        [Verb("export-csv", HelpText = "Exports indexes as csv.")]
        class ExportCSVOptions
        {
            [Option('f',@"file")]
            public string FileName { get; set; }
        }

        static int Main(string[] args)
        {
            var returned = CommandLine.Parser.Default.ParseArguments<AdviceOptions, ListIndexesOptions, EvalOptions, ShowCandlesOptions, RichOptions, RefreshCachedIndexesOptions, AddIndexesToCacheOptions, ExportCSVOptions>(args)
              .MapResult(
                (AdviceOptions opts) => RunAdvoceAndReturnExitCode(opts),
                (ListIndexesOptions opts) => RunListIndexesAndReturnExitCode(opts),
                (EvalOptions opts) => RunEvalAndReturnExitCode(opts),
                (ShowCandlesOptions opts) => RunShowCandlesAndReturnExitCode(opts),
                (RichOptions opts) => RunRichAndReturnExitCode(opts),
                (RefreshCachedIndexesOptions opts) => RunRefreshCachedIndexesAndReturnExitCode(opts),
                (AddIndexesToCacheOptions opts) => RunAddIndexesToCacheAndReturnExitCode(opts),
                (ExportCSVOptions opts) => RunExportCSVAndReturnExitCode(opts),
                errs => 1);

            Console.WriteLine(@"press any key to finish...");
            Console.ReadKey();
            return returned;
        }

        private static int RunAdvoceAndReturnExitCode(AdviceOptions opts)
        {
            EvaluationStep step = stockLogic.LetsBecomeRich(opts.Index);
            Console.WriteLine(step.ToString());
            return 0;
        }

        private static int RunListIndexesAndReturnExitCode(ListIndexesOptions opts)
        {
            foreach (var value in Enum.GetValues(typeof(EIndex)))
            {
                Console.Write(((int)value).ToString().PadLeft(7).PadRight(8));
                Console.WriteLine(value.ToString());
            }
            return 0;
        }

        private static int RunEvalAndReturnExitCode(EvalOptions opts)
        {
            var evaluation = stockLogic.Eval(opts.Index, opts.Bollinger, opts.MACD, opts.RSI).ToArray();
            var bollinger = stockLogic.EvalIndicator(Constants.Bollinger, opts.Index).ToArray();
            var macd = stockLogic.EvalIndicator(Constants.MACD, opts.Index).ToArray();
            var rsi = stockLogic.EvalIndicator(Constants.RSI, opts.Index).ToArray();
            var count = evaluation.Count();
            for(int i = 0; i< count;i++)
            {
                var e = evaluation[i];
                Console.Write(e.Date.ToShortDateString());
                Console.Write(e.Valid ? " valid     " : " not valid ");
                Console.WriteLine(e.Value);
                Console.Write(@" Bollinger=");
                Console.Write(bollinger[i].Value);
                Console.Write(@" MACD=");
                Console.Write(macd[i].Value);
                Console.Write(@" RSI=");
                Console.WriteLine(rsi[i].Value);
            }
            return 0;
        }

        private static int RunShowCandlesAndReturnExitCode(ShowCandlesOptions opts)
        {
            investingContext.GetCandles(paramIndex: (EIndex)opts.Index).ToList().ForEach(c =>
            {
                Console.Write(c.Date.ToShortDateString());
                Console.Write(' ');
                Console.Write(Math.Round(c.Low, 5));
                Console.Write(' ');
                Console.WriteLine(Math.Round(c.High, 5));
            });
            return 0;
        }

        private static int RunRichAndReturnExitCode(RichOptions opts)
        {
            stockLogic.LetsBecomeRich();
            return 0;
        }

        private static int RunAddIndexesToCacheAndReturnExitCode(AddIndexesToCacheOptions opts)
        {
            stockLogic.AddIndexesToCache(opts.StartId, opts.EndId);
            return 0;
        }

        private static int RunExportCSVAndReturnExitCode(ExportCSVOptions opts)
        {
            stockLogic.ExportCSV(opts.FileName);
            return 0;
        }

        private static int RunRefreshCachedIndexesAndReturnExitCode(RefreshCachedIndexesOptions opts)
        {
            stockLogic.RefreshStoredIndexes();
            return 0;
        }
    }
}
