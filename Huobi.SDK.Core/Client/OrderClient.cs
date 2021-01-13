using System.Threading.Tasks;
using Huobi.SDK.Core.RequestBuilder;
using Huobi.SDK.Model.Response.Order;
using Huobi.SDK.Model.Request.Order;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Collections.Generic;

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

    public class OrderManager
    {
        [System.Serializable]
        public struct Order
        {
            public string symbol;
            public enum Type { buy, sell };

            public Type type;

            public float price;

            public float amount;

            private long id;
            private OrderManager orders;

            public Order(OrderManager orders, long id)
            {
                this.orders = orders;
                this.id = id;
                symbol = "";
                type = Type.buy;
                price = -1;
                amount = -1;
            }

            public void Cancel()
            {
                orders.CancelOrder(id);
            }
        }

        public OrderClient orderClient;
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public int orderUpdatePeriod;

        List<Order> orders_ = new List<Order>();
        public Order[] orders { get { return orders_.ToArray(); } }

        string accountId = "";

        /// <summary>
        /// Cancel Order Async
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action">failed callback, call when failed.</param>
        public void CancelOrder(long id, System.Action<string> failed = null)
        {
            orderClient.CancelOrderByIdAsync(id.ToString(), (a, b, c) => { if (b != null) failed?.Invoke(b); });
        }

        /// <summary>
        /// Place Order Async
        /// </summary>
        /// <param name="symbol">trade symbol</param>
        /// <param name="type">type</param>
        /// <param name="amount">buy/sell amout, when use market price, this is amout to sell, or amout used to by</param>
        /// <param name="price">price, -1 to use market price</param>
        /// <param name="retryNum">retry if failed</param>
        /// <param name="callback">order if suc</param>
        public void PlaceOrder(string symbol, Order.Type type, float amount, float price = -1, int retryNum = 0, System.Action<System.Nullable<Order>> callback = null)
        {
            if (accountId != "")
            {
                var request = new Huobi.SDK.Model.Request.Order.PlaceOrderRequest()
                {
                    AccountId = accountId,
                    amount = amount.ToString(),
                    source = "spot-api",
                    price = price < 0 ? "0" : price.ToString(),
                    symbol = symbol,
                    type = type.ToString() + (price < 0 ? "-market" : "-limit")
                };

                System.Action<int> task = null;
                task = (remain) =>
                {
                    orderClient.PlaceOrderAsync(request,
                        (id, ec, em) =>
                        {
                            if (ec == null && em == null)
                            {
                                Order order = new Order(this, long.Parse(id))
                                {
                                    amount = amount,
                                    price = price,
                                    symbol = symbol,
                                    type = type,
                                };

                                callback?.Invoke(order);
                            }
                            else if (remain-- > 0)
                            {
                                Console.WriteLine(ec + "\n" + em + "\nretry: " + remain);
                                task(remain);
                            }
                            else
                            {
                                Console.WriteLine(ec + "\n" + em);
                                callback?.Invoke(null);
                            }
                        });
                };
                task(retryNum);
            }
            else
            {
                Console.WriteLine("connection is invalid, try later");
                callback?.Invoke(null);
            }
        }

        public void PlaceOrder(Order order, int retryNum = 0, System.Action<System.Nullable<Order>> callback = null)
        {
            PlaceOrder(order.symbol, order.type, order.amount, order.price, retryNum, callback);
        }

        async void UpdateOrders()
        {
            CancellationToken token = tokenSource.Token;
            await Task.Run(() =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested) return;

                    orderClient.GetOpenOrdersAsync(new Huobi.SDK.Core.GetRequest(), (orders, errorCode, errorMsg) => {

                        List<Order> orders__ = new List<Order>();
                        foreach (var order in orders)
                        {
                            var o = new Order(this, order.id)
                            {
                                amount = float.Parse(order.amount),
                                price = float.Parse(order.price),
                                symbol = order.symbol
                            };
                            o.type = order.type.Contains("buy") ? Order.Type.buy : Order.Type.sell;

                            orders__.Add(o);
                        }
                        orders_ = orders__;
                    });

                    Thread.Sleep(orderUpdatePeriod);
                }
            });
        }

        public void Dispose()
        {
            tokenSource.Cancel();
        }

        public OrderManager(string acc, string sec, int orderUpdatePeriod = 1000)
        {
            this.orderUpdatePeriod = orderUpdatePeriod;
            HttpRequest.httpClient.Timeout = System.TimeSpan.FromSeconds(5);
            orderClient = new OrderClient(acc, sec);

            System.Action GetAcount = null;
            GetAcount = () =>
            {
                AccountClient accountClient = new AccountClient(acc, sec);
                accountClient.GetAccountInfoAsync((account, status) => {
                    if (status != "ok") GetAcount();
                    else
                    {
                        accountId = account[0].id.ToString();
                        Console.WriteLine("Order server connection finish.");
                    }
                });
            };
            GetAcount();

            UpdateOrders();
        }
    }
}
