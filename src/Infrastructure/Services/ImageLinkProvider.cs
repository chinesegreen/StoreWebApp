using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public static class ImageLinkProvider
    {
        public static async Task<string> SaveFile(IFormFile file)
        {
            string unique = Guid.NewGuid().ToString();
            string name = unique + file.FileName;
            string filePath = Path.Combine("wwwroot/img/products", name);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/img/products/{name}";
        }
    }
}
