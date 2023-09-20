using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DeliveryOrderProcessor;

public class DeliveryOrder
{
    private readonly ILogger _logger;

    public DeliveryOrder(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<DeliveryOrder>();
    }

    [Function("DeliveryOrder")]
    public async Task<DeliveryOrderResponse> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        
        var order = await JsonSerializer.DeserializeAsync<Order>(req.Body);

        return new DeliveryOrderResponse
        {
            Order = order,
            HttpResponse = req.CreateResponse(HttpStatusCode.OK)
        };
    }
}