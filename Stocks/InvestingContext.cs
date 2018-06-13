using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stocks.Entities;
using System.Net;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace Stocks
{
    public class InvestingContext : IInvestingContext
    {
        public IEnumerable<Candle> GetCandles(int paramCount = 70, EIndex paramIndex = EIndex.Ibex35, int paramIntervalInSeconds = 86400)
        {
            // Create a request for the URL. 
            HttpWebRequest request = (HttpWebRequest)(HttpWebRequest.Create(
              String.Format("http://www.investing.com/common/modules/js_instrument_chart/api/data.php?pair_id={2}&pair_id_for_news={2}&chart_type=candlestick&pair_interval={0}&candle_count={1}&events=yes&volume_series=yes",
              paramIntervalInSeconds,
              paramCount,
              (int)paramIndex)));
            request.Headers.Add(@"Pragma: no - cache");
            request.Headers.Add(@"X-Requested-With: XMLHttpRequest");
            request.Headers.Add(@"Cookie: optimizelyEndUserId=oeu1467020917805r0.45394119451797144; __gads=ID=de969a9d8fe47679:T=1467020918:S=ALNI_MaU-yep1-fB8DbnGaUgyIX8G2PRLw; cookieConsent=was-set; editionPostpone=1467201240862; PHPSESSID=te9dkd6si0opoo285lrgkvee92; fpros_popup=up; geoC=ES; show_big_billboard1=true; _gat=1; _gat_allSitesTracker=1; gtmFired=OK; notification_news_411577=9854; SideBlockUser=a%3A2%3A%7Bs%3A10%3A%22stack_size%22%3Ba%3A1%3A%7Bs%3A11%3A%22last_quotes%22%3Bi%3A8%3B%7Ds%3A6%3A%22stacks%22%3Ba%3A1%3A%7Bs%3A11%3A%22last_quotes%22%3Ba%3A3%3A%7Bi%3A0%3Ba%3A3%3A%7Bs%3A7%3A%22pair_ID%22%3Bs%3A5%3A%2244336%22%3Bs%3A10%3A%22pair_title%22%3Bs%3A0%3A%22%22%3Bs%3A9%3A%22pair_link%22%3Bs%3A27%3A%22%2Findices%2Fvolatility-s-p-500%22%3B%7Di%3A1%3Ba%3A3%3A%7Bs%3A7%3A%22pair_ID%22%3Bs%3A3%3A%22174%22%3Bs%3A10%3A%22pair_title%22%3Bs%3A0%3A%22%22%3Bs%3A9%3A%22pair_link%22%3Bs%3A17%3A%22%2Findices%2Fspain-35%22%3B%7Di%3A2%3Ba%3A3%3A%7Bs%3A7%3A%22pair_ID%22%3Bs%3A2%3A%2227%22%3Bs%3A10%3A%22pair_title%22%3Bs%3A0%3A%22%22%3Bs%3A9%3A%22pair_link%22%3Bs%3A15%3A%22%2Findices%2Fuk-100%22%3B%7D%7D%7D%7D; optimizelySegments=%7B%224225444387%22%3A%22gc%22%2C%224226973206%22%3A%22search%22%2C%224232593061%22%3A%22false%22%2C%225010352657%22%3A%22none%22%7D; optimizelyBuckets=%7B%7D; _ga=GA1.2.1028568263.1467020918");
            request.UserAgent = @"Mozilla / 5.0(Windows NT 6.1; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 51.0.2704.103 Safari / 537.36";
            request.Referer = @"http://www.investing.com/indices/spain-35";
            request.Host = @"www.investing.com";
            request.Accept = @"application/json, text/javascript, */*; q=0.01";
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseFromServer);
            foreach (var c in ((IEnumerable<dynamic>)data.candles))
            {
                yield return new Candle
                {
                    Date = (new DateTime(1970, 1, 1)).AddSeconds(Convert.ToDouble(c[0]) / 1000),
                    Open = Convert.ToDouble(c[1]),
                    High = Convert.ToDouble(c[2]),
                    Low = Convert.ToDouble(c[3]),
                    Close = Convert.ToDouble(c[4])
                };
            }
            // Clean up the streams and the response.
            reader.Close();
            response.Close();
        }

        public string GetIndexDescription(int paramIndex)
        {
            // Create a request for the URL. 
            HttpWebRequest request = (HttpWebRequest)(HttpWebRequest.Create(
              String.Format("http://www.investing.com/common/modules/js_instrument_chart/api/data.php?pair_id={2}&pair_id_for_news={2}&chart_type=candlestick&pair_interval={0}&candle_count={1}&events=yes&volume_series=yes",
              86400,
              70,
              paramIndex)));
            request.Headers.Add(@"Pragma: no - cache");
            request.Headers.Add(@"X-Requested-With: XMLHttpRequest");
            request.Headers.Add(@"Cookie: optimizelyEndUserId=oeu1467020917805r0.45394119451797144; __gads=ID=de969a9d8fe47679:T=1467020918:S=ALNI_MaU-yep1-fB8DbnGaUgyIX8G2PRLw; cookieConsent=was-set; editionPostpone=1467201240862; PHPSESSID=te9dkd6si0opoo285lrgkvee92; fpros_popup=up; geoC=ES; show_big_billboard1=true; _gat=1; _gat_allSitesTracker=1; gtmFired=OK; notification_news_411577=9854; SideBlockUser=a%3A2%3A%7Bs%3A10%3A%22stack_size%22%3Ba%3A1%3A%7Bs%3A11%3A%22last_quotes%22%3Bi%3A8%3B%7Ds%3A6%3A%22stacks%22%3Ba%3A1%3A%7Bs%3A11%3A%22last_quotes%22%3Ba%3A3%3A%7Bi%3A0%3Ba%3A3%3A%7Bs%3A7%3A%22pair_ID%22%3Bs%3A5%3A%2244336%22%3Bs%3A10%3A%22pair_title%22%3Bs%3A0%3A%22%22%3Bs%3A9%3A%22pair_link%22%3Bs%3A27%3A%22%2Findices%2Fvolatility-s-p-500%22%3B%7Di%3A1%3Ba%3A3%3A%7Bs%3A7%3A%22pair_ID%22%3Bs%3A3%3A%22174%22%3Bs%3A10%3A%22pair_title%22%3Bs%3A0%3A%22%22%3Bs%3A9%3A%22pair_link%22%3Bs%3A17%3A%22%2Findices%2Fspain-35%22%3B%7Di%3A2%3Ba%3A3%3A%7Bs%3A7%3A%22pair_ID%22%3Bs%3A2%3A%2227%22%3Bs%3A10%3A%22pair_title%22%3Bs%3A0%3A%22%22%3Bs%3A9%3A%22pair_link%22%3Bs%3A15%3A%22%2Findices%2Fuk-100%22%3B%7D%7D%7D%7D; optimizelySegments=%7B%224225444387%22%3A%22gc%22%2C%224226973206%22%3A%22search%22%2C%224232593061%22%3A%22false%22%2C%225010352657%22%3A%22none%22%7D; optimizelyBuckets=%7B%7D; _ga=GA1.2.1028568263.1467020918");
            request.UserAgent = @"Mozilla / 5.0(Windows NT 6.1; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 51.0.2704.103 Safari / 537.36";
            request.Referer = @"http://www.investing.com/indices/spain-35";
            request.Host = @"www.investing.com";
            request.Accept = @"application/json, text/javascript, */*; q=0.01";
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseFromServer);
            
            // Clean up the streams and the response.
            reader.Close();
            response.Close();

            return Regex.Match((string)(data.html.chart_info), @"<span[^>]*>([^<]*)<\/span>").Groups[1].Value;
        }

        public IEnumerable<Candle> GetCandlesEx(EIndex paramIndex = EIndex.Ibex35)
        {
            // Create a request for the URL. 
            HttpWebRequest request = (HttpWebRequest)(HttpWebRequest.Create(
              String.Format("http://tvc4.forexpros.com/be48f9f75fd3d0bd24c0c25fdac8e87d/1467353873/1/1/8/history?symbol={0}&resolution=D&from=1466489883&to=1467353883",
              (int)paramIndex)));
            request.Headers.Add(@"Pragma: no - cache");
            request.Headers.Add(@"Cookie: optimizelyEndUserId=oeu1467020917805r0.45394119451797144; __gads=ID=de969a9d8fe47679:T=1467020918:S=ALNI_MaU-yep1-fB8DbnGaUgyIX8G2PRLw; cookieConsent=was-set; editionPostpone=1467201240862; PHPSESSID=te9dkd6si0opoo285lrgkvee92; fpros_popup=up; geoC=ES; show_big_billboard1=true; _gat=1; _gat_allSitesTracker=1; gtmFired=OK; notification_news_411577=9854; SideBlockUser=a%3A2%3A%7Bs%3A10%3A%22stack_size%22%3Ba%3A1%3A%7Bs%3A11%3A%22last_quotes%22%3Bi%3A8%3B%7Ds%3A6%3A%22stacks%22%3Ba%3A1%3A%7Bs%3A11%3A%22last_quotes%22%3Ba%3A3%3A%7Bi%3A0%3Ba%3A3%3A%7Bs%3A7%3A%22pair_ID%22%3Bs%3A5%3A%2244336%22%3Bs%3A10%3A%22pair_title%22%3Bs%3A0%3A%22%22%3Bs%3A9%3A%22pair_link%22%3Bs%3A27%3A%22%2Findices%2Fvolatility-s-p-500%22%3B%7Di%3A1%3Ba%3A3%3A%7Bs%3A7%3A%22pair_ID%22%3Bs%3A3%3A%22174%22%3Bs%3A10%3A%22pair_title%22%3Bs%3A0%3A%22%22%3Bs%3A9%3A%22pair_link%22%3Bs%3A17%3A%22%2Findices%2Fspain-35%22%3B%7Di%3A2%3Ba%3A3%3A%7Bs%3A7%3A%22pair_ID%22%3Bs%3A2%3A%2227%22%3Bs%3A10%3A%22pair_title%22%3Bs%3A0%3A%22%22%3Bs%3A9%3A%22pair_link%22%3Bs%3A15%3A%22%2Findices%2Fuk-100%22%3B%7D%7D%7D%7D; optimizelySegments=%7B%224225444387%22%3A%22gc%22%2C%224226973206%22%3A%22search%22%2C%224232593061%22%3A%22false%22%2C%225010352657%22%3A%22none%22%7D; optimizelyBuckets=%7B%7D; _ga=GA1.2.1028568263.1467020918");
            request.UserAgent = @"Mozilla / 5.0(Windows NT 6.1; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 51.0.2704.103 Safari / 537.36";
            request.Referer = @"http://tvccdn.investing.com/web/360/index58-prod.html?carrier=be48f9f75fd3d0bd24c0c25fdac8e87d&time=1467353873&domain_ID=1&lang_ID=1&timezone_ID=8&version=360&locale=en&timezone=America/New_York&pair_ID=169&interval=D&session=session&prefix=www&suffix=&client=0&user=&family_prefix=tvc4&init_page=instrument&sock_srv=stream17.forexpros.com";
            request.Host = @"tvc4.forexpros.com";
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseFromServer);
            var t = ((IEnumerable<dynamic>)data.t).ToArray(); //time
            var o = ((IEnumerable<dynamic>)data.o).ToArray(); //open
            var c = ((IEnumerable<dynamic>)data.c).ToArray(); //close
            var h = ((IEnumerable<dynamic>)data.h).ToArray(); //high
            var l = ((IEnumerable<dynamic>)data.l).ToArray(); //low
            var v = ((IEnumerable<dynamic>)data.v).ToArray(); //volume
            for (int i=0; i<t.Length; i++)
            {
                yield return new Candle
                {
                    Date = (new DateTime(1970, 1, 1)).AddSeconds(Convert.ToDouble(t[i])),
                    Open = Convert.ToDouble(o[i]),
                    High = Convert.ToDouble(h[i]),
                    Low = Convert.ToDouble(l[i]),
                    Close = Convert.ToDouble(c[i])
                };
            }
            // Clean up the streams and the response.
            reader.Close();
            response.Close();
        }

        public IEnumerable<Index> GetIndexes(int paramStartId = 0, int paramEndId = 10000)
        {
            for (int i = paramStartId; i < paramEndId; i++)
            {
                var description = String.Empty;

                try
                {
                    description = GetIndexDescription(i);
                }
                catch { continue; }

                yield return new Index() { Id = i, Description = description };
            }
        }
    }
}
