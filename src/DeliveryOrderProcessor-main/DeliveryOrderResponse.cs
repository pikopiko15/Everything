using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace DeliveryOrderProcessor;

public class DeliveryOrderResponse
{
    [CosmosDBOutput("***", "***",
        ConnectionStringSetting = "***", CreateIfNotExists = true)]
    public Order Order { get; set; }
    public HttpResponseData HttpResponse { get; set; }
}