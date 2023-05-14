namespace BotyObchodASP.Models
{
    public class Editmodel
    {
        public TbProduct Product { get; set; }
        public List<TbStock> Stocks { get; set; }
        public List<TbCategoriesDetail> Categories { get; set; }

    }
}
