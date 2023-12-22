using System.ComponentModel.DataAnnotations;

namespace EComMVC.Models.BO
{
    public class Panier
    {
        [Key]
        public int Id { get; set; }
        public int IdProduct { get; set; }  
        public int IdUser { get; set; } 

    }
}
