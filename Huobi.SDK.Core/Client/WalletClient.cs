using System.Threading.Tasks;
using Huobi.SDK.Core.RequestBuilder;
using Huobi.SDK.Model.Response.Wallet;
using Huobi.SDK.Model.Request.Wallet;

namespace Huobi.SDK.Core.Client
{
    /// <summary>
    /// Responsible to operate wallet
    /// </summary>
    public class WalletClient
    {
        private const string GET_METHOD = "GET";
        private const string POST_METHOD = "POST";

        private const string DEFAULT_HOST = "api.huobi.pro";

        private readonly PrivateUrlBuilder _urlBuilder;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accessKey">Access Key</param>
        /// <param name="secretKey">Secret Key</param>
        /// <param name="host">the host that the client connects to</param>
        public WalletClient(string accessKey, string secretKey, string host = DEFAULT_HOST)
        {
            _urlBuilder = new PrivateUrlBuilder(accessKey, secretKey, host);
        }

        /// <summary>
        /// Get deposit address of corresponding chain, for a specific crypto currency (except IOTA)
        /// <para>Address[] data</para>
        /// <para>int code //Status code</para>
        /// <para>string message //Error message (if any)</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetDepositAddressResponse</returns>
        public void GetDepositAddressAsync(GetRequest request,
                                            System.Action<GetDepositAddressResponse.Address[], int, string> action)
        {
            string url = _urlBuilder.Build(GET_METHOD, "/v2/account/deposit/address", request);

            HttpRequest.GetAsync<GetDepositAddressResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.code, res.message);
            });
        }

        /// <summary>
        /// Query withdraw quota for currencies
        /// <para>Quota data</para>
        /// <para>int code //Status code</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetWithdrawQuotaResponse</returns>
        public void GetWithdrawQuotaAsync(GetRequest request,
                                            System.Action<GetWithdrawQuotaResponse.Quota, int> action)
        {
            string url = _urlBuilder.Build(GET_METHOD, "/v2/account/withdraw/quota", request);

            HttpRequest.GetAsync<GetWithdrawQuotaResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.code);
            });
        }

        /// <summary>
        /// Get withdraw address
        /// <para>Address[] data</para>
        /// <para>int code //Status code</para>
        /// <para>string message //Error message (if any)</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetDepositAddressResponse</returns>
        public void GetWithdrawAddressAsync(GetRequest request,
                                            System.Action<GetDepositAddressResponse.Address[], int, string> action)
        {
            string url = _urlBuilder.Build(GET_METHOD, "/v2/account/withdraw/address", request);

            HttpRequest.GetAsync<GetDepositAddressResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.code, res.message);
            });
        }


        /// <summary>
        /// Withdraw from spot trading account to an external address.
        /// <para>long data // Transfer id</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>WithdrawCurrencyResponse</returns>
        public void WithdrawCurrencyAsync(WithdrawRequest request,
                                            System.Action<long, string, string> action = null)
        {
            string url = _urlBuilder.Build(POST_METHOD, "/v1/dw/withdraw/api/create");

            HttpRequest.PostAsync<WithdrawCurrencyResponse>(url, request.ToJson()).ContinueWith((task) =>
            {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.errorCode, res.errorMessage);
                }
            });
        }

        /// <summary>
        /// Cancels a previously created withdraw request by its transfer id.
        /// <para>long data // Transfer id</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="withdrawId">The transfer id returned when create withdraw request</param>
        /// <returns>CancelWithdrawCurrencyResponse</returns>
        public void CancelWithdrawCurrencyAsync(long withdrawId,
                                                System.Action<long, string, string> action = null)
        {
            string url = _urlBuilder.Build(POST_METHOD, $"/v1/dw/withdraw-virtual/{withdrawId}/cancel");

            HttpRequest.PostAsync<CancelWithdrawCurrencyResponse>(url).ContinueWith((task) =>
            {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.errorCode, res.errorMessage);
                }
            });
        }

        /// <summary>
        /// Returns all existed withdraws and deposits and return their latest status.
        /// <para>History[] data // Transfer id</para>
        /// <para>string status</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetDepositWithdrawHistoryResponse</returns>
        public void GetDepositWithdrawHistoryAsync(GetRequest request,
                                                    System.Action<GetDepositWithdrawHistoryResponse.History[], string> action)
        {
            string url = _urlBuilder.Build(GET_METHOD, "/v1/query/deposit-withdraw", request);

            HttpRequest.GetAsync<GetDepositWithdrawHistoryResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.status);
            });
        }
    }
}
