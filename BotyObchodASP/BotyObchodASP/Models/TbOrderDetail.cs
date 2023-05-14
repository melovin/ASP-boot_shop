using System;
using System.Collections.Generic;

namespace BotyObchodASP
{
    public partial class TbOrderDetail
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int IdStock { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public int Tax { get; set; }

        public virtual TbOrder IdOrderNavigation { get; set; } = null!;
        public virtual TbStock IdStockNavigation { get; set; } = null!;
    }
}
