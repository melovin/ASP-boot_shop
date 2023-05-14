namespace BotyObchodASP.Models
{
    public class CartPrep
    {
        public TbOrder Order { get; set; }
        public List<TbOrderDetail> OrderDetail { get; set; } = new();
        public TbCustomer Customer { get; set; } = new();
    }
}
