namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.CustomViews;

public partial class CustomPopupPagoRealizado : ContentPage
{
    private int countdownSeconds = 10;
    private bool isCountdownRunning = false;

    public CustomPopupPagoRealizado()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        StartCountdown();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is DateTime deliveryTime)
        {
            labelHoraEstimada.Text = deliveryTime.ToString("h:mm tt");
        }
    }

    private void StartCountdown()
    {
        if (!isCountdownRunning)
        {
            isCountdownRunning = true;
            _ = RunCountdown();
        }
    }

    private async Task RunCountdown()
    {
        while (countdownSeconds > 0)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                labelSegundos.Text = $"Porfavor espere mientras es redireccionado ({countdownSeconds}s)";
            });

            await Task.Delay(1000); // Wait for 1 second
            countdownSeconds--;
        }

        await Navigation.PushAsync(new Views.Pedidos.pedidosPrincipal());
    }
}