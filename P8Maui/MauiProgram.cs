using Microsoft.Extensions.Logging;
using P8Maui.ViewModels;

namespace P8Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // --- Injection de dépendances ---------------------------------------
        // Fichier SQLite dans le dossier privé de l'application : il survit
        // aux redémarrages (persistance des données).
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "people.db3");

        // Singleton : une seule connexion SQLite pour toute l'application.
        builder.Services.AddSingleton(_ => new PersonRepository(dbPath));
        builder.Services.AddSingleton<MainViewModel>();

        // Pages : Shell les crée via ce conteneur, ce qui permet à MainPage
        // de recevoir son MainViewModel dans son constructeur.
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
    }
}
