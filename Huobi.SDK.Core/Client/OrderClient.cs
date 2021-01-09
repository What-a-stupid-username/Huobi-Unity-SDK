using System.Threading.Tasks;
using Huobi.SDK.Core.RequestBuilder;
using Huobi.SDK.Model.Response.Order;
using Huobi.SDK.Model.Request.Order;
using Newtonsoft.Json;
using System;

namespace Huobi.SDK.Core.Client
{
    /// <summary>
    /// Responsible to operate on order
    /// </summary>
    public class OrderClient
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
        public OrderClient(string accessKey, string secretKey, string host = DEFAULT_HOST)
        {
            _urlBuilder = new PrivateUrlBuilder(accessKey, secretKey, host);
        }

        /// <summary>
        /// Place a new order and send to the exchange to be matched.
        /// <para>string data // Order id</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>PlaceOrderResponse</returns>
        public void PlaceOrderAsync(PlaceOrderRequest request,
                                            System.Action<string, string, string> action = null)
        {
            string url = _urlBuilder.Build(POST_METHOD, "/v1/order/orders/place");

            HttpRequest.PostAsync<PlaceOrderResponse>(url, request.ToJson()).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.errorCode, res.errorMessage);
                }
            });
        }

        /// <summary>
        /// Place multipler orders (at most 10 orders)
        /// <para>PlaceOrderResult[] data</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="requests"></param>
        /// <returns>PlaceOrdersResponse</returns>
        public void PlaceOrdersAsync(PlaceOrderRequest[] requests,
                                                                    System.Action<PlaceOrdersResponse.PlaceOrderResult[], string, string> action = null)
        {
            string url = _urlBuilder.Build(POST_METHOD, "/v1/order/batch-orders");

            HttpRequest.PostAsync<PlaceOrdersResponse>(url, JsonConvert.SerializeObject(requests)).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.errorCode, res.errorMessage);
                }
            });
        }

        /// <summary>
        /// Cancel an order by order id
        /// <para>string data // Order id</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>CancelOrderByIdResponse</returns>
        public void CancelOrderByIdAsync(string orderId,
                                            System.Action<string, string, string> action = null)
        {
            string url = _urlBuilder.Build(POST_METHOD, $"/v1/order/orders/{orderId}/submitcancel");            

            HttpRequest.PostAsync<CancelOrderByIdResponse>(url).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.errorCode, res.errorMessage);
                }
            });
        }

        /// <summary>
        /// Cancel an order by client order id
        /// <para>int data // Cancellation status code</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="clientOrderId">Client order id</param>
        /// <returns>CancelOrderByClientResponse</returns>
        public void CancelOrderByClientOrderIdAsync(string clientOrderId,
                                                        System.Action<int, string, string> action = null)
        {
            string url = _urlBuilder.Build(POST_METHOD, "/v1/order/orders/submitCancelClientOrder");

            string body = $"{{ \"client-order-id\":\"{clientOrderId}\" }}";

            HttpRequest.PostAsync<CancelOrderByClientResponse>(url, body).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.errorCode, res.errorMessage);
                }
            });
        }


        /// <summary>
        /// Returns all open orders which have not been filled completely.
        /// <para>OpenOrder[] data</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetOpenOrdersResponse</returns>
        public void GetOpenOrdersAsync(GetRequest request,
                                        System.Action<GetOpenOrdersResponse.OpenOrder[], string, string> action = null)
        {
            string url = _urlBuilder.Build(GET_METHOD, "/v1/order/openOrders", request);

            HttpRequest.GetAsync<GetOpenOrdersResponse>(url).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.errorCode, res.errorMessage);
                }
            });
        }

        /// <summary>
        /// Submit cancellation for multiple orders at once with given criteria.
        /// <para>Body data</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>CancelOrdersByCriteriaResponse</returns>
        public void CancelOrdersByCriteriaAsync(CancelOrdersByCriteriaRequest request,
                                                System.Action<CancelOrdersByCriteriaResponse.Body, string, string> action = null)
        {
            string url = _urlBuilder.Build(POST_METHOD, $"/v1/order/orders/batchCancelOpenOrders");

            HttpRequest.PostAsync<CancelOrdersByCriteriaResponse>(url, request.ToJson()).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.errorCode, res.errorMessage);
                }
            });
        }

        /// <summary>
        /// Submit cancellation for multiple orders at once with given ids
        /// <para>Body data</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>CancelOrdersByIdsResponse</returns>
        public void CancelOrdersByIdsAsync(CancelOrdersByIdsRequest request,
                                                System.Action<CancelOrdersByIdsResponse.Body, string, string> action = null)
        {
            string url = _urlBuilder.Build(POST_METHOD, $"/v1/order/orders/batchcancel");

            HttpRequest.PostAsync<CancelOrdersByIdsResponse>(url, request.ToJson()).ContinueWith((task) => {
                if (action != null)
                {
                    var res = task.Result;
                    action(res.data, res.errorCode, res.errorMessage);
                }
            });
        }

        /// <summary>
        /// Returns the detail of one order by order id
        /// <para>Order data</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <returns>GetOrderByIdResponse</returns>
        public void GetOrderByIdAsync(string orderId,
                                        System.Action<GetOrderResponse.Order, string, string> action)
        {
            string url = _urlBuilder.Build(GET_METHOD, $"/v1/order/orders/{orderId}");

            HttpRequest.GetAsync<GetOrderResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.errorCode, res.errorMessage);
            });
        }

        /// <summary>
        /// Returns the detail of one order by client order id
        /// <para>Order data</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetOrderByIdResponse</returns>
        public void GetOrderByClientAsync(GetRequest request,
                                            System.Action<GetOrderResponse.Order, string, string> action)
        {
            string url = _urlBuilder.Build(GET_METHOD, $"/v1/order/orders/getClientOrder", request);

            HttpRequest.GetAsync<GetOrderResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.errorCode, res.errorMessage);
            });
        }

        /// <summary>
        /// Returns the match result of an order.
        /// <para>MatchResult[] data</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>GetMatchResultsResponse</returns>
        public void GetMatchResultsByIdAsync(string orderId,
                                            System.Action<GetMatchResultsResponse.MatchResult[], string, string> action)
        {
            string url = _urlBuilder.Build(GET_METHOD, $"/v1/order/orders/{orderId}/matchresults");

            HttpRequest.GetAsync<GetMatchResultsResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.errorCode, res.errorMessage);
            });
        }

        /// <summary>
        /// Returns orders based on a specific searching criteria.
        /// <para>HistoryOrder[] data</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetHistoryOrdersResponse</returns>
        public void GetHistoryOrdersAsync(GetRequest request,
                                            System.Action<GetHistoryOrdersResponse.HistoryOrder[], string, string> action)
        {
            string url = _urlBuilder.Build(GET_METHOD, $"/v1/order/orders", request);

            HttpRequest.GetAsync<GetHistoryOrdersResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.errorCode, res.errorMessage);
            });
        }

        /// <summary>
        /// Returns orders based on a specific searching criteria.
        /// Note: queriable range should be within past 1 day for cancelled order (state = "canceled")
        /// <para>HistoryOrder[] data</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetHistoryOrdersResponse</returns>
        public void GetLast48hOrdersAsync(GetRequest request,
                                            System.Action<GetHistoryOrdersResponse.HistoryOrder[], string, string> action)
        {
            string url = _urlBuilder.Build(GET_METHOD, $"/v1/order/history", request);

            HttpRequest.GetAsync<GetHistoryOrdersResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.errorCode, res.errorMessage);
            });
        }

        /// <summary>
        /// Returns the match results of past and open orders based on specific search criteria.
        /// <para>MatchResult[] data</para>
        /// <para>string errorCode</para>
        /// <para>string errorMessage</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetMatchResultsResponse</returns>
        public void GetMatchResultsAsync(GetRequest request,
                                            System.Action<GetMatchResultsResponse.MatchResult[], string, string> action)
        {
            string url = _urlBuilder.Build(GET_METHOD, $"/v1/order/matchresults", request);

            HttpRequest.GetAsync<GetMatchResultsResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.errorCode, res.errorMessage);
            });
        }

        /// <summary>
        /// Returns the current transaction fee rate applied to the user.
        /// <para>Fee[] data</para>
        /// <para>int code</para>
        /// <para>string message</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetTransactFeeRateResponse</returns>
        public void GetTransactFeeRateAsync(GetRequest request,
                                    System.Action<GetTransactFeeRateResponse.Fee[], int, string> action)
        {
            string url = _urlBuilder.Build(GET_METHOD, $"/v2/reference/transact-fee-rate", request);

            HttpRequest.GetAsync<GetTransactFeeRateResponse>(url).ContinueWith((task) => {
                var res = task.Result;
                action(res.data, res.code, res.message);
            });
        }
    }
}
