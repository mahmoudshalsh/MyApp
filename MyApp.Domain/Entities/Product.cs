namespace MyApp.Domain.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }

        public int CategoryID { get; set; }
        public virtual Category? Category { get; set; }
    }
}