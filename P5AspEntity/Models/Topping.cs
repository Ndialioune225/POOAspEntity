namespace P5AspEntity.Models;

// Bonus : deuxième entité ajoutée après InitialCreate, pour montrer
// l'évolution du schéma avec une seconde migration ("AddToppings").
public class Topping
{
    public int Id { get; set; }

    public string? Name { get; set; }

    // Supplément de prix en francs CFA.
    public decimal Price { get; set; }
}
