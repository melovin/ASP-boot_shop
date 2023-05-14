using System;
using System.Collections.Generic;

namespace BotyObchodASP
{
    public partial class TbProduct
    {
        public TbProduct()
        {
            TbCategoriesDetails = new HashSet<TbCategoriesDetail>();
            TbPictures = new HashSet<TbPicture>();
            TbStocks = new HashSet<TbStock>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string DetailedDescription { get; set; } = null!;
        public string Material { get; set; } = null!;
        public string Type { get; set; } = null!;
        public bool Active { get; set; }

        public virtual ICollection<TbCategoriesDetail> TbCategoriesDetails { get; set; }
        public virtual ICollection<TbPicture> TbPictures { get; set; }
        public virtual ICollection<TbStock> TbStocks { get; set; }
    }
}
