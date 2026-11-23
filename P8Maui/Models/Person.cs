using SQLite;

namespace P8Maui.Models;

// Une ligne de la table "people" dans la base SQLite locale (module 8 du parcours MAUI).
[Table("people")]
public class Person
{
    // Clé primaire auto-incrémentée par SQLite.
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // Nom unique : SQLite refuse deux personnes avec le même nom.
    [MaxLength(250), Unique]
    public string Name { get; set; } = string.Empty;
}
