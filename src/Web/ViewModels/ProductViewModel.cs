﻿using Core.Entities;
using Web.BindingModels;

namespace Web.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public List<string>? Images { get; set; }

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
