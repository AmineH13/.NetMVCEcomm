using System.ComponentModel.DataAnnotations.Schema;

namespace EComMVC.Models
{
    public class AddProductViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public double Price { get; set; }

        public string? ImageFile { get; set; }


    }
}
