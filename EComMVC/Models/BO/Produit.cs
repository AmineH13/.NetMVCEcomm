using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EComMVC.Models.BO
{
    public class Produit
    {

        [Key]

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
            
        public double Price { get; set; }


        public string? Image {  get; set; }


    }
}
