using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Services
{
    using System.Net;

    public class PaymentListener
    {
        private HttpListener _listener;
     

        public PaymentListener(int port)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://localhost:{port}/");
        }

        public async Task StartListeningAsync(Action<string> onSuccess, Action<string> onCancel)
        {
            _listener.Start();

            while (_listener.IsListening)
            {
                var context = await _listener.GetContextAsync();
                var request = context.Request;

                if (request.Url.AbsolutePath == "/success")
                {
                    var sessionId = request.QueryString["session_id"];
                    onSuccess(sessionId);
                    await SendResponse(context.Response, 200, "Platnosc udana, mozesz zamknac te karte.");
                }
                else if (request.Url.AbsolutePath == "/cancel")
                {
                    var sessionId = request.QueryString["session_id"];
                    onCancel(sessionId);
                    await SendResponse(context.Response, 200, "Payment successfull, now you can close this tab.");
                }
            }
        }

        private async Task SendResponse(HttpListenerResponse response, int statusCode, string message)
        {
            response.StatusCode = statusCode;
            var buffer = Encoding.UTF8.GetBytes(message);
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();
        }

        public void Stop()
        {
            _listener.Stop();
        }
    }
}
