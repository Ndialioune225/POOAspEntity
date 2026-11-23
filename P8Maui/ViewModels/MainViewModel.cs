using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using P8Maui.Models;

namespace P8Maui.ViewModels;

// ViewModel de la page Personnes (modèle MVVM, modules 4 et 5 du parcours MAUI).
// La page XAML se lie à ces propriétés avec {Binding ...} : aucun code dans MainPage.xaml.cs
// ne manipule directement les contrôles.
public class MainViewModel : INotifyPropertyChanged
{
    private readonly PersonRepository _repository;
    private string _newName = string.Empty;
    private string _statusMessage = string.Empty;

    public MainViewModel(PersonRepository repository)
    {
        _repository = repository;

        // Une commande = une action déclenchée depuis la vue (clic sur un bouton).
        AddCommand = new Command(async () => await AddAsync());
        LoadCommand = new Command(async () => await LoadAsync());
        DeleteCommand = new Command<Person>(async person => await DeleteAsync(person));
    }

    // ObservableCollection prévient la CollectionView à chaque ajout/suppression.
    public ObservableCollection<Person> People { get; } = new();

    public string NewName
    {
        get => _newName;
        set => SetProperty(ref _newName, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public ICommand AddCommand { get; }
    public ICommand LoadCommand { get; }
    public ICommand DeleteCommand { get; }

    private async Task AddAsync()
    {
        try
        {
            var person = await _repository.AddNewPersonAsync(NewName);
            StatusMessage = $"{person.Name} a été ajouté(e).";
            NewName = string.Empty;
            await LoadAsync(updateStatus: false);
        }
        catch (ArgumentException ex)
        {
            StatusMessage = ex.Message;
        }
        catch (SQLite.SQLiteException)
        {
            StatusMessage = $"Impossible d'ajouter « {NewName} » : ce nom existe déjà.";
        }
    }

    public async Task LoadAsync(bool updateStatus = true)
    {
        var people = await _repository.GetAllPeopleAsync();
        People.Clear();
        foreach (var person in people)
            People.Add(person);

        if (updateStatus)
            StatusMessage = $"{People.Count} personne(s) dans la base.";
    }

    private async Task DeleteAsync(Person? person)
    {
        if (person is null) return;

        await _repository.DeletePersonAsync(person);
        People.Remove(person);
        StatusMessage = $"{person.Name} a été supprimé(e).";
    }

    // --- Plomberie INotifyPropertyChanged -------------------------------------
    public event PropertyChangedEventHandler? PropertyChanged;

    // Met à jour le champ et prévient la vue seulement si la valeur change.
    private void SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
