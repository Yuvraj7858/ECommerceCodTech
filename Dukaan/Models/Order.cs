public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public List<OrderItem> OrderItems { get; set; }
}
