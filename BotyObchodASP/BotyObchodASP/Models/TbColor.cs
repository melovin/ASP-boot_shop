using System;
using System.Collections.Generic;

namespace BotyObchodASP
{
    public partial class TbColor
    {
        public TbColor()
        {
            TbStocks = new HashSet<TbStock>();
        }

        public int Id { get; set; }
        public string HexCode { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<TbStock> TbStocks { get; set; }
    }
}
