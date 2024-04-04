/*
 * Descripci�n:
 * Este c�digo define la l�gica de backend para la p�gina 'VerificarContra' de la aplicaci�n Floristeria Margaritas, la cual maneja la verificaci�n del c�digo de restablecimiento de contrase�a.
 * Incluye validaci�n del c�digo de verificaci�n y redireccionamiento a la p�gina de restablecimiento de contrase�a.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class VerificarContra : ContentPage
{
    // Variables para el c�digo de verificaci�n y el correo electr�nico
    private string? codigoVerificacion;
    private string? Correo;

    // Instancia de ApiService para interacciones con la API
    private ApiService _apiService = new ApiService();

    //Conteo de Reenvio
    private int countdownSeconds = 30;
    private bool isCountdownRunning = false;

    // Modelo de datos para el ID de usuario
    public class idUsuarioModel
    {
        public int idusuario { get; set; }
    }

    // Constructor para la p�gina 'VerificarContra'
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

    // M�todo que se ejecuta al aparecer la p�gina
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is recuperarPassModel data)
        {
            codigoVerificacion = data.Codigo;
            Correo = data.Correo;
        }
    }

    // Controlador de eventos para clic en el bot�n de verificar c�digo
    private async void btnVerificar_Clicked(object sender, EventArgs e)
    {
        // Validar si se ingres� un c�digo de verificaci�n
        if (string.IsNullOrEmpty(entryCodigo.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese el c�digo de verificaci�n que recibi� en su correo electr�nico o pida un nuevo c�digo.", "OK");
            return;
        }

        // Verificar si el c�digo ingresado es correcto
        if (entryCodigo.Text == codigoVerificacion)
        {
            // Obtener el ID de usuario asociado al correo electr�nico
            var userDetails = await _apiService.PostDataAsync<idUsuarioModel>("obtenerIdUsuarioCorreo.php", new { correo = Correo });
            int idUsuario = userDetails.idusuario;

            var data = new clientIdModel
            {
                idcliente = idUsuario
            };

            // Redireccionar a la p�gina de restablecimiento de contrase�a
            await Navigation.PushAsync(new Views.Login.RestaurarContra { BindingContext = data });
        }
        else
        {
            // Mostrar alertas en caso de errores
            await DisplayAlert("Alerta", "El c�digo de verificaci�n ingresado es incorrecto porfavor ingr�selo de nuevo o pida un nuevo c�digo.", "OK");
            return;
        }
    }

    // Controlador de eventos para clic en el bot�n de retroceso
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    // M�todo para iniciar el conteo regresivo
    private void StartCountdown()
    {
        if (!isCountdownRunning)
        {
            isCountdownRunning = true;
            _ = RunCountdown();
        }
    }

    // M�todo para ejecutar el conteo regresivo
    private async Task RunCountdown()
    {
        // Ejecutar el conteo regresivo y actualizar la etiqueta correspondiente
        while (countdownSeconds > 0)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                labelEnviarCog.Text = $"Enviar de nuevo ({countdownSeconds}s)";
                labelEnviarCog.IsEnabled = false;
            });

            await Task.Delay(1000);
            countdownSeconds--;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            labelEnviarCog.Text = "Enviar de nuevo";
            labelEnviarCog.IsEnabled = true;
            isCountdownRunning = false;
            countdownSeconds = 30;
        });
    }

    // Controlador de eventos para toque en la etiqueta de reenv�o de c�digo
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var resultadoCorreo = await _apiService.PostDataAsync<codigoVerificacionModel>("correoCodigoVerificacionReestablecerPass.php", new { email = Correo, name = "Floristeria Margaritas" });
        codigoVerificacion = resultadoCorreo.verification_code;
        await DisplayAlert("Alerta", "C�digo de Verificaci�n Reenviado", "OK");
    }
}