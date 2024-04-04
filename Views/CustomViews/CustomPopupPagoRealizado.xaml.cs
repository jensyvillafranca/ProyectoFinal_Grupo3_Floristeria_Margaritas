/*
 * Descripción:
 * Este código define la lógica de backend para la vista emergente 'CustomPopupPagoRealizado' de la aplicación Floristeria Margaritas.
 * Muestra un mensaje de pago realizado y un contador de tiempo antes de redireccionar al usuario a la página principal de pedidos.
 */

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.CustomViews;

public partial class CustomPopupPagoRealizado : ContentPage
{
    private int countdownSeconds = 10;
    private bool isCountdownRunning = false;

    // Constructor
    public CustomPopupPagoRealizado()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        StartCountdown();
    }

    // Método que se ejecuta cuando la página se muestra
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Mostrar la hora estimada de entrega si se establece en el contexto de enlace
        if (BindingContext is DateTime deliveryTime)
        {
            labelHoraEstimada.Text = deliveryTime.ToString("h:mm tt");
        }
    }

    // Método para iniciar el contador de tiempo
    private void StartCountdown()
    {
        if (!isCountdownRunning)
        {
            isCountdownRunning = true;
            _ = RunCountdown();
        }
    }

    // Método asíncrono para ejecutar el contador de tiempo
    private async Task RunCountdown()
    {
        while (countdownSeconds > 0)
        {
            // Actualizar el texto del contador de tiempo en el hilo principal
            MainThread.BeginInvokeOnMainThread(() =>
            {
                labelSegundos.Text = $"Porfavor espere mientras es redireccionado ({countdownSeconds}s)";
            });

            await Task.Delay(1000); // Wait for 1 second
            countdownSeconds--;
        }

        // Redireccionar a la página principal de pedidos después de que el contador llegue a cero
        await Navigation.PushAsync(new Views.Pedidos.pedidosPrincipal());
    }
}