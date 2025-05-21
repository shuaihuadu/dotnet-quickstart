using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PrometheusDemo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class StreamingController : ControllerBase
    {
        [HttpGet("async")]
        public async Task GetAsyncStream()
        {
            Response.Headers.ContentType = "text/event-stream";
            Response.Headers.CacheControl = "no-cache";
            Response.Headers.Connection = "keep-alive";

            await Response.Body.FlushAsync();

            await foreach (var message in GenerateMessages())
            {
                await Response.WriteAsync(message);
                await Response.Body.FlushAsync(); // 关键：禁用缓冲
            }
        }

        private async IAsyncEnumerable<string> GenerateMessages()
        {
            while (!HttpContext.RequestAborted.IsCancellationRequested)
            {
                yield return $"data: {DateTime.Now}\n\n";
                await Task.Delay(300);
            }
        }

        [HttpGet("minimal")]
        public async Task MinimalStream()
        {
            Response.Headers.ContentType = "text/event-stream";
            Response.Headers.CacheControl = "no-cache";
            Response.Headers.Connection = "keep-alive";

            await Response.Body.FlushAsync();

            for (int i = 0; i < 5; i++)
            {
                await Response.WriteAsync($"data: Message {i}\n\n");
                await Response.Body.FlushAsync();
                await Task.Delay(300);
            }
        }
    }
}
