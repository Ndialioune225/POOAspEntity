using Microsoft.EntityFrameworkCore;
using P5AspEntity.Models;

namespace P5AspEntity.Data;

// Le DbContext fait le lien entre les objets C# et la base de données.
// Les options (fournisseur InMemory ou SQLite, chaîne de connexion...) sont
// passées par l'injection de dépendances depuis Program.cs.
public class PizzaDb : DbContext
{
    public PizzaDb(DbContextOptions<PizzaDb> options) : base(options) { }

    // Chaque DbSet correspond à une table.
    public DbSet<Pizza> Pizzas => Set<Pizza>();

    // Ajouté plus tard (bonus) : table créée par la migration "AddToppings".
    public DbSet<Topping> Toppings => Set<Topping>();
}
