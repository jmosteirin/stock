﻿using CommandLine;
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

        [Verb("show-candles", HelpText = "Shows the candles for a particular index.")]
        class ShowCandlesOptions
        {
            [Option('i', @"index")]
            public int Index { get; set; }
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
            var returned = CommandLine.Parser.Default.ParseArguments<ShowCandlesOptions, RichOptions, RefreshCachedIndexesOptions, AddIndexesToCacheOptions, ExportCSVOptions>(args)
              .MapResult(
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
