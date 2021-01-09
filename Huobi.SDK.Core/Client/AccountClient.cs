using System.Threading.Tasks;
using Huobi.SDK.Core.RequestBuilder;
using Huobi.SDK.Model.Request.Account;
using Huobi.SDK.Model.Response.Account;
using Huobi.SDK.Model.Response.Transfer;

namespace Huobi.SDK.Core.Client
{
    /// <summary>
    /// Responsible to operate account
    /// </summary>
    public class AccountClient
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
        public AccountClient(string accessKey, string secretKey, string host = DEFAULT_HOST)
        {
            _urlBuilder = new PrivateUrlBuilder(accessKey, secretKey, host);
        }

        /// <summary>
        /// Returns a list of accounts owned by this API user
        /// <para>AccountInfo[] data</para>
        /// <para>string status</para>
        /// </summary>
        /// <returns>GetAccountInfoResponse</returns>
        public void GetAccountInfoAsync(System.Action<GetAccountInfoResponse.AccountInfo[], string> action = null)
        {
            string url = _urlBuilder.Build(GET_METHOD, "/v1/account/accounts");

            HttpRequest.GetAsync<GetAccountInfoResponse>(url).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.status);
                }
            });
        }


        /// <summary>
        /// Returns the balance of an account specified by account id
        /// <para>Data data</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="accountId">account id</param>
        /// <returns>GetAccountBalanceResponse</returns>
        public void GetAccountBalanceAsync(string accountId, 
                                            System.Action<GetAccountBalanceResponse.Data, string, string> action = null)
        {
            string url = _urlBuilder.Build(GET_METHOD, $"/v1/account/accounts/{accountId}/balance");

            HttpRequest.GetAsync<GetAccountBalanceResponse>(url).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.errorCode, res.errorMessage);
                }
            });
        }

        /// <summary>
        /// Returns the balance of an account specified by account id
        /// <para>Data data</para>
        /// <para>int code //Status code</para>
        /// <para>string message //Error message (if any)</para>
        /// </summary>
        /// <param name="accountType">The type of this account</param>
        /// <param name="valuationCurrency">The valuation according to the certain fiat currency</param>
        /// <param name="subUid">Sub User's UID.</param>
        /// <returns>GetAccountAssetValuationResponse</returns>
        public void GetAccountAssetValuationAsync(string accountType, string valuationCurrency = "BTC", long subUid = 0,
                                                    System.Action<GetAccountAssetValuationResponse.Data, int, string> action = null)
        {
            var request = new GetRequest()
                .AddParam("accountType", accountType);

            if (!string.IsNullOrEmpty(valuationCurrency))
            {
                request.AddParam("valuationCurrency", valuationCurrency);
            }
            if (subUid != 0)
            {
                request.AddParam("subUid", subUid.ToString());
            }

            string url = _urlBuilder.Build(GET_METHOD, $"/v2/account/asset-valuation", request);

            HttpRequest.GetAsync<GetAccountAssetValuationResponse>(url).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.code, res.message);
                }
            });
        }

        /// <summary>
        /// Parent user and sub user transfer asset between accounts.
        /// <para>TransferResponse data</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="request">TransferAccountRequest</param>
        /// <returns>TransferAccountResponse</returns>
        public void TransferAccountAsync(TransferAccountRequest request, 
                                            System.Action<TransferAccountResponse.TransferResponse, string, string> action = null)
        {
            string url = _urlBuilder.Build(POST_METHOD, "/v1/account/transfer");

            HttpRequest.PostAsync<TransferAccountResponse>(url, request.ToJson()).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.erroCode, res.erroMessage);
                }
            });
        }

        /// <summary>
        /// Returns the amount changes of specified user's account
        /// <para>History[] data</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetAccountHistoryResponse</returns>
        public void GetAccountHistoryAsync(GetRequest request, 
                                            System.Action<GetAccountHistoryResponse.History[], string, string> action = null)
        {
            string url = _urlBuilder.Build(GET_METHOD, "/v1/account/history", request);

            HttpRequest.GetAsync<GetAccountHistoryResponse>(url).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.errorCode, res.errorMessage);
                }
            });
        }

        /// <summary>
        /// Returns the amount changes of specified user's account
        /// <para>Ledger[] data</para>
        /// <para>long nextId</para>
        /// <para>int code</para>
        /// <para>string message</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetAccountHistoryResponse</returns>
        public void GetAccountLedgerAsync(GetRequest request, 
                                            System.Action<GetAccountLedgerResponse.Ledger[], long, int, string> action = null)
        {
            string url = _urlBuilder.Build(GET_METHOD, "/v2/account/ledger", request);

            HttpRequest.GetAsync<GetAccountLedgerResponse>(url).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.nextId, res.code, res.message);
                }
            });
        }

        /// <summary>
        /// Transfer fund from spot account to futrue contract account.
        /// <para>long data // Transfer id</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="currency">Currency name</param>
        /// <param name="amount">Amount of fund to transfer	</param>
        /// <returns>TransferResponse</returns>
        public void TransferFromSpotToFutureAsync(string currency, decimal amount, 
                                                    System.Action<long, string, string> action = null)
        {
            TransferSpotAndFutureAsync(currency, amount, "pro-to-futures", action);
        }

        /// <summary>
        /// Transfer fund from future contract account spot account.
        /// <para>long data // Transfer id</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="currency">Currency name</param>
        /// <param name="amount">Amount of fund to transfer</param>
        /// <returns>TransferResponse</returns>
        public void TransferFromFutureToSpotAsync(string currency, decimal amount, 
                                                    System.Action<long, string, string> action = null)
        {
            TransferSpotAndFutureAsync(currency, amount, "futures-to-pro", action);
        }

        /// <summary>
        /// transfer fund between spot account and future contract account
        /// <para>long data // Transfer id</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="currency">Currency name</param>
        /// <param name="amount">Amount of fund to transfer</param>
        /// <param name="type">Type of the transfer, possible values: [futures-to-pro, pro-to-futures]</param>
        /// <returns>TransferResponse</returns>
        private void TransferSpotAndFutureAsync(string currency, decimal amount, string type,
                                                    System.Action<long, string, string> action = null)
        {
            string url = _urlBuilder.Build(POST_METHOD, "/v1/futures/transfer");

            string content = $"{{ \"currency\": \"{currency}\", \"amount\":\"{amount}\", \"type\":\"{type}\" }}";

            HttpRequest.PostAsync<TransferResponse>(url, content).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.errorCode, res.errorMessage);
                }
            });
        }

        /// <summary>
        /// Returns the point balance of specified user
        /// <para>Data data</para>
        /// <para>int code //Status code</para>
        /// <para>string message //Error message (if any)</para>
        /// </summary>
        /// <param name="subUid"></param>
        /// <returns>GetPointBalanceResponse</returns>
        public void GetPointBalanceAsync(string subUid = null,
                                            System.Action<GetPointBalanceResponse.Data, int, string> action = null)
        {
            GetRequest request = null;

            if (!string.IsNullOrEmpty(subUid))
            {
                request = new GetRequest();
                request.AddParam("subUid", subUid);
            }

            string url = _urlBuilder.Build(GET_METHOD, "/v2/point/account", request);

            HttpRequest.GetAsync<GetPointBalanceResponse>(url).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.code, res.message);
                }
            });
        }

        /// <summary>
        /// transfer point between parent user and sub user
        /// <para>Data data</para>
        /// <para>int code //Status code</para>
        /// <para>string message //Error message (if any)</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>TransferResponse</returns>
        public void TransferPointAsync(TransferPointRequest request,
                                            System.Action<TransferPointResponse.Data, int, string> action = null)
        {
            string url = _urlBuilder.Build(POST_METHOD, "/v2/point/transfer");

            HttpRequest.PostAsync<TransferPointResponse>(url, request.ToJson()).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.code, res.message);
                }
            });
        }
    }
}
