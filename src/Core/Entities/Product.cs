using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public string? Subtitle { get; set; }
        public string? ImageLink { get; set; }
        public List<Additional>? Additionals { get; set; }
        public List<Tag>? Tags { get; set; }
        public bool IsTrending { get; set; }
        public bool IsDeleted { get; set; }
    }
}
