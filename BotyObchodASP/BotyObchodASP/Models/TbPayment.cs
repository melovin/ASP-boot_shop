using System;
using System.Collections.Generic;

namespace BotyObchodASP
{
    public partial class TbPayment
    {
        public TbPayment()
        {
            TbOrders = new HashSet<TbOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Price { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<TbOrder> TbOrders { get; set; }
    }
}
