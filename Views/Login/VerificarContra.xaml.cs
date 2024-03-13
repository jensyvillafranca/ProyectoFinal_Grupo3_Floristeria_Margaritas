using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class VerificarContra : ContentPage
{
    private string? codigoVerificacion;
    private string? Correo;

    private ApiService _apiService = new ApiService();

    //Conteo de Reenvio
    private int countdownSeconds = 30;
    private bool isCountdownRunning = false;

    public class idUsuarioModel
    {
        public int idusuario { get; set; }
    }


    public VerificarContra()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;

        labelEnviarCog.Text = "Enviar de nuevo";
        labelEnviarCog.GestureRecognizers.Add(new TapGestureRecognizer
        {
            NumberOfTapsRequired = 1,
            Command = new Command(StartCountdown)
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is recuperarPassModel data)
        {
            codigoVerificacion = data.Codigo;
            Correo = data.Correo;
        }
    }

    private async void btnVerificar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(entryCodigo.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese el código de verificación que recibió en su correo electrónico o pida un nuevo código.", "OK");
            return;
        }

        if (entryCodigo.Text == codigoVerificacion)
        {
            var userDetails = await _apiService.PostDataAsync<idUsuarioModel>("obtenerIdUsuarioCorreo.php", new { correo = Correo });
            int idUsuario = userDetails.idusuario;

            var data = new clientIdModel
            {
                idcliente = idUsuario
            };

            await Navigation.PushAsync(new Views.Login.RestaurarContra { BindingContext = data });
        }
        else
        {
            await DisplayAlert("Alerta", "El código de verificación ingresado es incorrecto porfavor ingréselo de nuevo o pida un nuevo código.", "OK");
            return;
        }
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
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
                labelEnviarCog.Text = $"Enviar de nuevo ({countdownSeconds}s)";
                labelEnviarCog.IsEnabled = false; // Disable the label during countdown
            });

            await Task.Delay(1000); // Wait for 1 second
            countdownSeconds--;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            labelEnviarCog.Text = "Enviar de nuevo";
            labelEnviarCog.IsEnabled = true; // Enable the label after countdown finishes
            isCountdownRunning = false;
            countdownSeconds = 30; // Reset the countdown for the next time
        });
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var resultadoCorreo = await _apiService.PostDataAsync<codigoVerificacionModel>("correoCodigoVerificacionReestablecerPass.php", new { email = Correo, name = "Floristeria Margaritas" });
        codigoVerificacion = resultadoCorreo.verification_code;
        await DisplayAlert("Alerta", "Código de Verificación Reenviado", "OK");
    }
}