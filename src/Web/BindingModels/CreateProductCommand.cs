using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Web.BindingModels
{
    public class CreateProductCommand
    {
        public int Id { get; set; }
        public string Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
    }
}
