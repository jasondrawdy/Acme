namespace AcmeCorpApi.Models 
{
  public class Order 
  {
    public int Id { get; set; }
    public int CustomerId {get; set;}
    public int ProductId {get; set;}
    public string ShippingAddress { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
  }
}