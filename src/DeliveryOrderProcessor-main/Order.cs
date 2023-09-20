namespace DeliveryOrderProcessor;

public class Order
{
    public ShippingAddress ShippingAddress { get; set; }
    public List<int> Items { get; set; }
    public decimal FinalPrice { get; set; }
}