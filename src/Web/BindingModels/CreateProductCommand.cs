using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Web.BindingModels
{
    public class CreateProductCommand
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Subtitle { get; set; }
        public List<string>? Tags { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsTrending { get; set; } = false;
        public IFormFile? Image { get; set; }
        public List<IFormFile>? Additionals { get; set; }
    }
}
