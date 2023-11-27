using Core.Entities;

namespace Web.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        public List<string> GetImages()
        {
            var images = new List<string>();

            if (Product.Images != null && Product.Images.Any())
            {
                foreach (var image in Product.Images)
                {
                    images.Add(image.Image);
                }
            }

            return images;
        }

        public List<string> GetCategories()
        {
            var categories = new List<string>();

            if (Product.Categories != null && Product.Categories.Any())
            {
                foreach (var category in Product.Categories)
                {
                    categories.Add(category.Name);
                }
            }
            else
            {
                categories.Add("No categories");
            }

            return categories;
        }
    }
}
