namespace EComMVC.Models
{
    public class ProductPanierModel
    {
        public List<EComMVC.Models.BO.Produit>? Produits { get; set; }
        public int NumberProductInPanier { get; set; }
        public int IdUser { get; set; } 
    }
}
