using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StreamingController : ControllerBase
{
    private readonly ILogger<StreamingController> _logger;

    public StreamingController(ILogger<StreamingController> logger)
    {
        _logger = logger;
    }


    [HttpGet(Name = "async")]
    public async Task GetAsyncStream()
    {
        Response.Headers.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";

        await Response.Body.FlushAsync();

        await foreach (var message in GenerateMessages())
        {
            await Response.WriteAsync(message);
            await Response.Body.FlushAsync(); // ¹Ø¼ü£º½ûÓÃ»º³å
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
}
