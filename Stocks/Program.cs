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

        [Verb("refresh", HelpText = "Refresh the indexes cache.")]
        class RefreshCachedIndexesOptions
        {
            [Value(0, MetaName = "Start Id", HelpText = "start id in investing.")]
            public int? StartId { get; set; }

            [Value(1, MetaName = "End Id", HelpText = "end id in investing.")]
            public int? EndId { get; set; }
        }

        static int Main(string[] args)
        {
            var returned = CommandLine.Parser.Default.ParseArguments<RefreshCachedIndexesOptions>(args)
              .MapResult(
                (RefreshCachedIndexesOptions opts) => RunRefreshCachedIndexesAndReturnExitCode(opts),
                errs => 1);

            Console.WriteLine(@"press any key to finish...");
            Console.ReadKey();
            return returned;
        }

        private static int RunRefreshCachedIndexesAndReturnExitCode(RefreshCachedIndexesOptions opts)
        {
            stockLogic.RefreshStoredIndexes(opts.StartId.Value, opts.EndId.Value);
            return 0;
        }
    }
}
