using Core.Entities;

namespace Web.ViewModels
{
    public class AdminProductsViewModel
    {
        public List<Product> Products { get; set; }
        public int NumberOfPages { get; set; }
    }
}
