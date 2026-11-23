using P8Maui.ViewModels;

namespace P8Maui;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    // Le ViewModel est fourni automatiquement par l'injection de dépendances (MauiProgram.cs).
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    // À chaque affichage de la page, on recharge la liste depuis SQLite :
    // les données sont donc visibles même après un redémarrage de l'application.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}
