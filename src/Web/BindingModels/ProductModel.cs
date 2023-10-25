using System.ComponentModel.DataAnnotations;

namespace Web.BindingModels
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
