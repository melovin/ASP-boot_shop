using System;
using System.Collections.Generic;

namespace BotyObchodASP
{
    public partial class TbCategory
    {
        public TbCategory()
        {
            TbCategoriesDetails = new HashSet<TbCategoriesDetail>();
        }

        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; } = null!;
        public int Position { get; set; }
        public int Level { get; set; }
        public int LeftIndex { get; set; }
        public int RightIndex { get; set; }

        public virtual ICollection<TbCategoriesDetail> TbCategoriesDetails { get; set; }
    }
}
