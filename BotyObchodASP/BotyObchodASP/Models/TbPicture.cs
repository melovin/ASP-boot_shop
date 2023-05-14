using System;
using System.Collections.Generic;

namespace BotyObchodASP
{
    public partial class TbPicture
    {
        public int Id { get; set; }
        public int? IdStock { get; set; }
        public string Path { get; set; } 
        public bool PrimaryImg { get; set; }
        public int? IdProduct { get; set; }

        public virtual TbProduct? IdProductNavigation { get; set; }
        public virtual TbStock? IdStockNavigation { get; set; }
    }
}
