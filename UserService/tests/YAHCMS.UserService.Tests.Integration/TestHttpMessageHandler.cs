using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class TestHttpMessageHandler : DelegatingHandler
    {
        private ILogger _logger;

        public TestHttpMessageHandler(ILogger logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Sending HTTP message using TestHttpMessageHandler. Uri: '{request.RequestUri.ToString()}'");

            if (WrappedMessageHandler == null) throw new Exception("You must set WrappedMessageHandler before TestHttpMessageHandler can be used.");
            var method = typeof(HttpMessageHandler).GetMethod("SendAsync", BindingFlags.Instance | BindingFlags.NonPublic);
            var result = method.Invoke(this.WrappedMessageHandler, new object[] { request, cancellationToken });
            return await (Task<HttpResponseMessage>)result;
        }

        public HttpMessageHandler WrappedMessageHandler { get; set; }
    }