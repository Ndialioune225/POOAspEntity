using P8Maui.Models;
using SQLite;

namespace P8Maui;

// Accès aux données : toutes les lectures/écritures SQLite passent par ici.
// Le ViewModel ne connaît pas SQLite, il appelle seulement ces méthodes.
public class PersonRepository
{
    private readonly string _dbPath;
    private SQLiteAsyncConnection? _conn;

    // Le chemin du fichier est fourni par MauiProgram (injection de dépendances).
    public PersonRepository(string dbPath)
    {
        _dbPath = dbPath;
    }

    // Ouvre la connexion et crée la table au premier appel seulement.
    private async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        if (_conn is not null)
            return _conn;

        _conn = new SQLiteAsyncConnection(_dbPath);
        await _conn.CreateTableAsync<Person>();
        return _conn;
    }

    public async Task<Person> AddNewPersonAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Un nom valide est requis.", nameof(name));

        var conn = await GetConnectionAsync();
        var person = new Person { Name = name.Trim() };
        // Peut lever SQLiteException si le nom existe déjà (contrainte Unique).
        await conn.InsertAsync(person);
        return person; // Id est rempli par SQLite après l'insertion.
    }

    public async Task<List<Person>> GetAllPeopleAsync()
    {
        var conn = await GetConnectionAsync();
        return await conn.Table<Person>().OrderBy(p => p.Name).ToListAsync();
    }

    public async Task DeletePersonAsync(Person person)
    {
        var conn = await GetConnectionAsync();
        await conn.DeleteAsync(person);
    }
}
