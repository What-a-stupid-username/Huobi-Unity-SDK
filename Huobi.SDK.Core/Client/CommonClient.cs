using System.Threading.Tasks;
using Huobi.SDK.Core.RequestBuilder;
using Huobi.SDK.Model.Response.Common;

namespace Huobi.SDK.Core.Client
{
    /// <summary>
    /// Responsible to get common information
    /// </summary>
    public class CommonClient
    {
        private const string DEFAULT_HOST = "api.huobi.pro";

        private PublicUrlBuilder _urlBuilder;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="host">the host that the client connects to</param>
        public CommonClient(string host = DEFAULT_HOST)
        {
            _urlBuilder = new PublicUrlBuilder(host);
        }

        /// <summary>
        /// Get system status, Incidents and planned maintenance.
        /// </summary>
        /// <returns></returns>
        public void GetSystemStatus(System.Action<string> action)
        {
            string url = "https://status.huobigroup.com/api/v2/summary.json";

            HttpRequest.GetStringAsync(url).ContinueWith((task) => { action(task.Result); });
        }

        /// <summary>
        /// Returns current market status
        /// <para>Status data</para>
        /// <para>int code //Status code</para>
        /// <para>string message //Error message (if any)</para>
        /// </summary>
        /// <returns>GetMarketStatusResponse</returns>
        public void GetMarketStatusAsync(System.Action<GetMarketStatusResponse.Status, int, string> action)
        {
            string url = _urlBuilder.Build("/v2/market-status");

            HttpRequest.GetAsync<GetMarketStatusResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.code, res.message); 
            });
        }

        /// <summary>
        /// Get all Huobi's supported trading symbol.
        /// <para>Symbol[] data</para>
        /// <para>string status</para>
        /// </summary>
        /// <returns>GetSymbolsResponse</returns>
        public void GetSymbolsAsync(System.Action<GetSymbolsResponse.Symbol[], string> action)
        {
            string url = _urlBuilder.Build("/v1/common/symbols");

            HttpRequest.GetAsync<GetSymbolsResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.status);
            });
        }


        /// <summary>
        /// Get all Huobi's supported trading currencies
        /// <para>string[] data</para>
        /// <para>string status</para>
        /// </summary>
        /// <returns>GetCurrencysResponse</returns>
        public void GetCurrencysAsync(System.Action<string[], string> action)
        {
            string url = _urlBuilder.Build("/v1/common/currencys");

            HttpRequest.GetAsync<GetCurrencysResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.status);
            });
        }

        /// <summary>
        /// Get currency information
        /// <para>Currency[] data</para>
        /// <para>int code //Status code</para>
        /// <para>string message //Error message (if any)</para>
        /// </summary>
        /// <param name="currency">currency name</param>
        /// <returns>GetCurrencyResponse</returns>
        public void GetCurrencyAsync(string currency, bool authorizedUser, System.Action<GetCurrencyResponse.Currency[], int, string> action)
        {
            string url = _urlBuilder.Build($"/v2/reference/currencies?currency={currency}&authorizedUser={authorizedUser}");

            HttpRequest.GetAsync<GetCurrencyResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.code, res.message);
            });
        }

        /// <summary>
        /// The current system time in milliseconds adjusted to Singapore time zone.
        /// </summary>
        /// <returns>GetTimestampResponse</returns>
        public void GetTimestampAsync(System.Action<string> action)
        {
            string url = _urlBuilder.Build("/v1/common/timestamp");

            HttpRequest.GetAsync<GetTimestampResponse>(url).ContinueWith((task) => { action(task.Result.data); });
        }

        /// <summary>
        /// Get currency information
        /// </summary>
        /// <param name="millisecond">Time out(ms)</param>
        /// <returns>GetCurrencyResponse</returns>
        public void SetTimeOutMS(double millisecond)
        {
            HttpRequest.httpClient.Timeout = System.TimeSpan.FromMilliseconds(millisecond);
        }
    }
}
