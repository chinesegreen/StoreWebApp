using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Web.BindingModels
{
    public class CreateProductCommand
    {
        public int Price { get; set; }
        public int? PriceWithoutDiscount { get; set; }
        public string Name { get; set; }
        public string VendorCode { get; set; }
        public string? Description { get; set; }
        public string? Subtitle { get; set; }
        public List<string>? Tags { get; set; }
        public int ValueTax { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool? IsTrending { get; set; } = false;
        public List<IFormFile> Images { get; set; }
        public Dimensions? Dimensions { get; set; }
    }
}
