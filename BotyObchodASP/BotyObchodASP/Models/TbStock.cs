using System;
using System.Collections.Generic;

namespace BotyObchodASP
{
    public partial class TbStock
    {
        public TbStock()
        {
            TbOrderDetails = new HashSet<TbOrderDetail>();
            TbPictures = new HashSet<TbPicture>();
        }

        public int Id { get; set; }
        public int IdProduct { get; set; }
        public int IdColor { get; set; }
        public int Size { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public string Code { get; set; }
        public int Tax { get; set; }
        public bool Active { get; set; }

        public virtual TbColor IdColorNavigation { get; set; } = null!;
        public virtual TbProduct IdProductNavigation { get; set; } = null!;
        public virtual ICollection<TbOrderDetail> TbOrderDetails { get; set; }
        public virtual ICollection<TbPicture> TbPictures { get; set; }
    }
}
