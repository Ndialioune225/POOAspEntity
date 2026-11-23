using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using P5AspEntity.Data;
using P5AspEntity.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------------
// ÉTAPE 1 du module Learn : base de données EN MÉMOIRE.
// Les données sont perdues à chaque redémarrage : pratique pour débuter.
// Pour la réactiver, décommentez la ligne suivante et commentez l'étape 2.
//
// builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase("items"));
// ---------------------------------------------------------------------------

// ---------------------------------------------------------------------------
// ÉTAPE 2 du module Learn : base SQLite persistante (fichier Pizzas.db).
// La chaîne de connexion est lue dans appsettings.json ("ConnectionStrings:Pizzas").
// Le schéma est créé par les migrations EF Core (dossier Migrations/).
// ---------------------------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("Pizzas") ?? "Data Source=Pizzas.db";
builder.Services.AddSqlite<PizzaDb>(connectionString);

// Swagger : génère la documentation de l'API et une page de test sur /swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PizzaStore API",
        Description = "Faire les pizzas que vous aimez",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore API V1"));

app.MapGet("/", () => "Hello World! Ouvrez /swagger pour tester l'API.");

// GET /pizzas : liste toutes les pizzas.
app.MapGet("/pizzas", async (PizzaDb db) => await db.Pizzas.ToListAsync());

// GET /pizza/{id} : une pizza, ou 404 si elle n'existe pas.
app.MapGet("/pizza/{id}", async (PizzaDb db, int id) =>
    await db.Pizzas.FindAsync(id) is Pizza pizza
        ? Results.Ok(pizza)
        : Results.NotFound());

// POST /pizza : crée une pizza et renvoie 201 Created avec son adresse.
app.MapPost("/pizza", async (PizzaDb db, Pizza pizza) =>
{
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();
    return Results.Created($"/pizza/{pizza.Id}", pizza);
});

// PUT /pizza/{id} : met à jour une pizza existante.
app.MapPut("/pizza/{id}", async (PizzaDb db, Pizza updatePizza, int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null) return Results.NotFound();

    pizza.Name = updatePizza.Name;
    pizza.Description = updatePizza.Description;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// DELETE /pizza/{id} : supprime une pizza.
app.MapDelete("/pizza/{id}", async (PizzaDb db, int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null) return Results.NotFound();

    db.Pizzas.Remove(pizza);
    await db.SaveChangesAsync();
    return Results.Ok();
});

// --- Bonus : garnitures (deuxième modèle, deuxième migration) ---------------
app.MapGet("/toppings", async (PizzaDb db) => await db.Toppings.ToListAsync());

app.MapPost("/topping", async (PizzaDb db, Topping topping) =>
{
    await db.Toppings.AddAsync(topping);
    await db.SaveChangesAsync();
    return Results.Created($"/topping/{topping.Id}", topping);
});

app.Run();
