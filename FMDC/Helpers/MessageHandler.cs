using System.Diagnostics;
using System.Text;
using System.Threading;

namespace FMDC.Helpers
{
    public class MessageMiddleware
    {

        private readonly ILogger<MessageMiddleware> _logger;
        private readonly RequestDelegate _next;

        public MessageMiddleware(RequestDelegate next, ILogger<MessageMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBodyStream = context.Request.Body;
            var request = await GetRequestAsText(context.Request);

            _logger.LogInformation(request);

            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);
            var response = await GetResponseAsText(context.Response);

            _logger.LogInformation(response);
            await responseBody.CopyToAsync(originalBodyStream);

        }

        private static async Task<string> GetResponseAsText(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            //Create stream reader to write entire stream
            var text = await new StreamReader(response.Body).ReadToEndAsync(CancellationToken.None);
            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }

        private static async Task<string> GetRequestAsText(HttpRequest request)
        {
            var body = request.Body;
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength, System.Globalization.CultureInfo.InvariantCulture)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //Assign the read body back to the request body
            request.Body = body;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";

        }
    }

}
