using System;
using System.Collections.Generic;

namespace BotyObchodASP
{
    public partial class TbCategoriesDetail
    {
        public int Id { get; set; }
        public int IdProduct { get; set; }
        public int IdCategory { get; set; }

        public virtual TbCategory IdCategoryNavigation { get; set; } = null!;
        public virtual TbProduct IdProductNavigation { get; set; } = null!;
    }
}
