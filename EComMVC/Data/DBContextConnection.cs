using EComMVC.Models.BO;
using Microsoft.EntityFrameworkCore;

namespace EComMVC.Data
{
    public class DBContextConnection : DbContext
    {
        public DBContextConnection(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Produit> produits { get; set; }
        public DbSet<User> users { get; set; }

        public DbSet<Panier> paniers { get; set; }
    }
}
