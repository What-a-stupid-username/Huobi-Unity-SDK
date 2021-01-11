using Huobi.SDK.Core.Client.WebSocketClientBase;
using Huobi.SDK.Core.Log;
using Huobi.SDK.Model.Response.Market;
using System;
using System.Threading.Tasks;
using UnityEngine;
using WebSocketSharp;

namespace Huobi.SDK.Core.Client
{
    /// <summary>
    /// Responsible to handle candlestick data from WebSocket
    /// </summary>
    public class CandlestickWebSocketClient : WebSocketClientBase<SubscribeCandlestickResponse>
    {
        void ErrorHandler(object sender, ErrorEventArgs e)
        {
            UnityEngine.Debug.LogError(e.Message + "\n" + (e.Exception != null ? e.Exception.Message : ""));
            throw e.Exception ?? new Exception("Unknown");
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="host">websockethost</param>
        public CandlestickWebSocketClient(EventHandler<ErrorEventArgs> errorHandler = null, string host = DEFAULT_HOST)
            :base(host)
        {
            _WebSocket.OnError += errorHandler ?? ErrorHandler;
        }

        /// <summary>
        /// Request the full candlestick data according to specified criteria
        /// </summary>
        /// <param name="symbol">Trading symbol</param>
        /// <param name="period">Candlestick internval
        /// possible values: 1min, 5min, 15min, 30min, 60min, 4hour, 1day, 1mon, 1week, 1year</param>
        /// <param name="from">From timestamp in second</param>
        /// <param name="to">To timestamp in second</param>
        /// <param name="clientId">Client id</param>
        public void Req(string symbol, string period, int from, int to, string clientId = "")
        {
            string topic = $"market.{symbol}.kline.{period}";

            _WebSocket.Send($"{{ \"req\": \"{topic}\",\"id\": \"{clientId}\", \"from\":{from}, \"to\":{to} }}");

            //_logger.Log(LogLevel.Info, $"WebSocket requested, topic={topic}, clientId={clientId}");
        }

        /// <summary>
        /// Subscribe candlestick data
        /// </summary>
        /// <param name="symbol">Trading symbol</param>
        /// <param name="period">Candlestick internval</param>
        /// <param name="clientId">Client id</param>
        public void Subscribe(string symbol, string period, string clientId = "")
        {
            string topic = $"market.{symbol}.kline.{period}";

            _WebSocket.Send($"{{ \"sub\": \"{topic}\",\"id\": \"{clientId}\" }}");

            //_logger.Log(LogLevel.Info, $"WebSocket subscribed, topic={topic}, clientId={clientId}");
        }

        /// <summary>
        /// Unsubscribe candlestick data
        /// </summary>
        /// <param name="symbol">Trading symbol</param>
        /// <param name="period">Candlestick interval</param>
        /// <param name="clientId">Client id</param>
        public void UnSubscribe(string symbol, string period, string clientId = "")
        {
            string topic = $"market.{symbol}.kline.{period}";

            _WebSocket.Send($"{{ \"unsub\": \"{topic}\",\"id\": \"{clientId}\" }}");

            //_logger.Log(LogLevel.Info, $"WebSocket unsubscribed, topic={topic}, clientId={clientId}");
        }
    }




    public class SubscribCandlestick
    {
        public bool valid { get; private set; } = false;
        public string name { get; private set; }
        public System.DateTime updateTime { get; private set; }
        public SubscribeCandlestickResponse.Tick tick { get; private set; }

        CandlestickWebSocketClient candlestickCilent;
        string period;

        void ErrorHandler(object sender, ErrorEventArgs e)
        {
            Debug.LogError(e.Message + (e.Exception != null ? e.Exception.Message : ""));
            Connect(name, period);
        }

        async void Connect(string symbol, string period)
        {
            await Task.Run(() =>
            {
                int try_num = 10;
                while (try_num-- > 0)
                {
                    try
                    {
                        candlestickCilent = new CandlestickWebSocketClient(/*ErrorHandler*/);
                        candlestickCilent.OnConnectionOpen += () => {
                            candlestickCilent.Subscribe(symbol, period);
                        };
                        candlestickCilent.OnResponseReceived += ReciveFromCandlestickCilent;
                        candlestickCilent.Connect();
                        valid = true;
                        break;

                    }
                    catch (Exception) { Debug.LogWarning("Connect failed. Retry" + try_num); }
                }
            });
        }

        async void Disconnect()
        {
            await Task.Run(() =>
            {
                if (valid)
                {
                    candlestickCilent.Disconnect();
                    valid = false;
                }
            });
        }

        public SubscribCandlestick(string symbol, string period = "1min")
        {
            Connect(symbol, period);
            name = symbol;
            updateTime = System.DateTime.MinValue;
            tick = new SubscribeCandlestickResponse.Tick();
            this.period = period;
        }

        public void Dispose()
        {
            Disconnect();
        }

        void ReciveFromCandlestickCilent(SubscribeCandlestickResponse response)
        {
            if (response.tick != null)
            {
                tick = response.tick;
                updateTime = System.DateTime.Now;
            }
        }
    }
}
