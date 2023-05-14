using System;
using System.Collections.Generic;

namespace BotyObchodASP
{
    public partial class TbOrder
    {
        public TbOrder()
        {
            TbOrderDetails = new HashSet<TbOrderDetail>();
        }

        public int Id { get; set; }
        public int IdCustomer { get; set; }
        public int IdDelivery { get; set; }
        public int IdPayment { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string State { get; set; }

        public virtual TbCustomer IdCustomerNavigation { get; set; } = null!;
        public virtual TbDelivery IdDeliveryNavigation { get; set; } = null!;
        public virtual TbPayment IdPaymentNavigation { get; set; } = null!;
        public virtual ICollection<TbOrderDetail> TbOrderDetails { get; set; }
    }
}
