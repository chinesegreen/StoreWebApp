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
        public string NormalizedName { get; set; }
        public string VendorCode { get; set; }
        public string NormalizedVendorCode { get; set; }
        public int Price { get; set; }
        public int? PriceWithoutDiscount { get; set; }
        public string? Description { get; set; }
        public string? Subtitle { get; set; }
        public string ImageLink { get; set; }
        public List<Additional>? Additionals { get; set; }
        public List<Tag>? Tags { get; set; }
        public bool IsTrending { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int QuantityInStock { get; set; } = 0;
        public int ValueTax { get; set; }
        public Dimensions? Dimensions { get; set; }

        public bool IsInStock()
        {
            return QuantityInStock > 0;
        }
    }
}
