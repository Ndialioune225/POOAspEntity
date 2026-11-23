namespace P5AspEntity.Models;

// Entité "Pizza" : EF Core en fait une table "Pizzas".
// Par convention, la propriété "Id" devient la clé primaire (auto-incrémentée).
public class Pizza
{
    public int Id { get; set; }

    // "?" = la valeur peut être null (Nullable activé dans le .csproj).
    public string? Name { get; set; }

    public string? Description { get; set; }
}
