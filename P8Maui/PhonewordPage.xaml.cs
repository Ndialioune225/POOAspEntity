namespace P8Maui;

public partial class PhonewordPage : ContentPage
{
    private string? _translatedNumber;

    public PhonewordPage()
    {
        InitializeComponent();
    }

    private void OnTranslate(object? sender, EventArgs e)
    {
        _translatedNumber = PhonewordTranslator.ToNumber(PhoneNumberText.Text);

        if (!string.IsNullOrEmpty(_translatedNumber))
        {
            CallButton.IsEnabled = true;
            CallButton.Text = "Appeler le " + _translatedNumber;
        }
        else
        {
            CallButton.IsEnabled = false;
            CallButton.Text = "Appeler";
        }
    }

    private async void OnCall(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_translatedNumber))
            return;

        var confirm = await DisplayAlertAsync(
            "Composer un numéro",
            "Voulez-vous appeler le " + _translatedNumber + " ?",
            "Oui", "Non");
        if (!confirm)
            return;

        // Le composeur n'existe pas sur tous les appareils (ex. PC Windows).
        if (PhoneDialer.Default.IsSupported)
            PhoneDialer.Default.Open(_translatedNumber);
        else
            await DisplayAlertAsync("Indisponible", "Cet appareil ne peut pas passer d'appel.", "OK");
    }
}
