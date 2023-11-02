namespace Web.BindingModels
{
    public class AddImagesCommand
    {
        public int ProductId { get; set; }
        public IFormFile Image { get; set; }
        public List<IFormFile> Additional { get; set; }
    }
}
