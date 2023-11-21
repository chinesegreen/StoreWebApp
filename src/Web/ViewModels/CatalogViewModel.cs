using Core.Entities;

namespace Web.ViewModels
{
    public class CatalogViewModel
    {
        public List<Product> Products { get; set; }

        public int NumberOfPages { get; set; }
    }
}
