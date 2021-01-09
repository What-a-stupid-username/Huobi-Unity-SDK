using System.Threading.Tasks;
using Huobi.SDK.Core.RequestBuilder;
using Huobi.SDK.Model.Response.Order;
using Huobi.SDK.Model.Response.Market;

namespace Huobi.SDK.Core.Client
{
    /// <summary>
    /// Responsible to get market information
    /// </summary>
    public class MarketClient
    {
        private const string DEFAULT_HOST = "api.huobi.pro";

        private PublicUrlBuilder _urlBuilder;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="host">the host that the client connects to</param>
        public MarketClient(string host = DEFAULT_HOST)
        {
            _urlBuilder = new PublicUrlBuilder(host);
        }

        /// <summary>
        /// Retrieves all klines in a specific range.
        /// <para>Candlestick[] data</para>
        /// <para>string status</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetCandlestickResponse</returns>
        public void GetCandlestickAsync(GetRequest request,
                                            System.Action<GetCandlestickResponse.Candlestick[], string> action)
        {
            string url = _urlBuilder.Build("/market/history/kline", request);

            HttpRequest.GetAsync<GetCandlestickResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.status);
            });
        }

        /// <summary>
        /// Retrieves the latest ticker with some important 24h aggregated market data.
        /// <para>Tick tick</para>
        /// <para>string status</para>
        /// </summary>
        /// <param name="symbol">Trading symbol</param>
        /// <returns>GetLast24hCandlestickAskBidResponse</returns>
        public void GetLast24hCandlestickAskBidAsync(string symbol,
                                            System.Action<GetLast24hCandlestickAskBidResponse.Tick, string> action)
        {
            string url = _urlBuilder.Build($"/market/detail/merged?symbol={symbol}");

            HttpRequest.GetAsync<GetLast24hCandlestickAskBidResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.tick, res.status);
            });
        }

        /// <summary>
        /// Retrieve the latest tickers for all supported pairs.
        /// <para>Candlestick[] data</para>
        /// <para>string status</para>
        /// </summary>
        /// <returns>GetLast24hCandlestickResponse</returns>
        public void GetAllSymbolsLast24hCandlesticksAskBidAsync(
                                            System.Action<GetAllSymbolsLast24hCandlesticksAskBidResponse.Candlestick[], string> action)
        {
            string url = _urlBuilder.Build("/market/tickers");

            HttpRequest.GetAsync<GetAllSymbolsLast24hCandlesticksAskBidResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.status);
            });
        }

        /// <summary>
        /// Retrieves the current order book of a specific pair
        /// <para>tickData tick</para>
        /// <para>string status</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetDepthResponse</returns>
        public void GetDepthAsync(GetRequest request,
                                    System.Action<GetDepthResponse.tickData, string> action)
        {
            string url = _urlBuilder.Build("/market/depth", request);

            HttpRequest.GetAsync<GetDepthResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.tick, res.status);
            });
        }

        /// <summary>
        /// Retrieves the latest trade with its price, volume, and direction.
        /// <para>Tick tick</para>
        /// <para>string status</para>
        /// </summary>
        /// <param name="symbol">Trading symbol</param>
        /// <returns>GetLastTradeResponse</returns>
        public void GetLastTradeAsync(string symbol,
                                        System.Action<GetLastTradeResponse.Tick, string> action)
        {
            string url = _urlBuilder.Build($"/market/trade?symbol={symbol}");

            HttpRequest.GetAsync<GetLastTradeResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.tick, res.status);
            });
        }

        /// <summary>
        /// Retrieves the most recent trades with their price, volume, and direction.
        /// <para>Tick[] data</para>
        /// <para>string status</para>
        /// </summary>
        /// <param name="symbol">Trading symbol</param>
        /// <param name="size">The number of data returns</param>
        /// <returns>GetLastTradesResponse</returns>
        public void GetLastTradesAsync(string symbol, int size,
                                        System.Action<GetLastTradesResponse.Tick[], string> action)
        {
            string url = _urlBuilder.Build($"/market/history/trade?symbol={symbol}&size={size}");

            HttpRequest.GetAsync<GetLastTradesResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.status);
            });
        }

        /// <summary>
        /// Retrieves the summary of trading in the market for the last 24 hours.
        /// <para>Candlestick tick</para>
        /// <para>string status</para>
        /// </summary>
        /// <param name="symbol">Trading symbol</param>
        /// <returns>GetLast24hCandlestickResponse</returns>
        public void GetLast24hCandlestickAsync(string symbol,
                                                System.Action<GetLast24hCandlestickResponse.Candlestick, string> action)
        {
            string url = _urlBuilder.Build($"/market/detail?symbol={symbol}");

            HttpRequest.GetAsync<GetLast24hCandlestickResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.tick, res.status);
            });
        }
    }
}
